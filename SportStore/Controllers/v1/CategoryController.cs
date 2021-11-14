using static SportStore.Helpers.ProductFiltring;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using SportStore.Managers.Repositories;
using SportStore.Models.Entities;
using System;
using AutoMapper;
using System.Net.Mime;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportStore.Models.Dtos;
using SportStore.Models.Results;
using Microsoft.AspNetCore.Http;
using SportStore.Extensions;

namespace SportStore.Controllers.v1
{
    [Route("/categories")]
    [ApiController]
    [ApiVersion("1.0")]
    [SwaggerTag("Create, update or delete categories.")]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    public class CategoryController : ControllerBase
    {
        private readonly IStoreRepository<Category> _categoryRepository;
        private readonly IStoreRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public CategoryController
            (
                IStoreRepository<Category> categoryRepository,
                IStoreRepository<Product> productRepository,
                IMapper mapper
            )
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        #region Endpoits

        // GET: /categories
        /// <summary>
        ///     Get all categories.
        /// </summary>
        /// <response code="200">Return all products.</response>
        /// <response code="default">Error!!</response>
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<CategoryResult>))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<CategoryResult>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var categoriesDTOs = _mapper.Map<IEnumerable<CategoryResult>>(categories);

            return Ok(categoriesDTOs);
        }

        //GET: /categories/{id:Guid}
        /// <summary>
        ///     Get Category with specific ID.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Return the category with the ID provided.</response>
        /// <response code="404">Category is not found.</response>
        /// <response code="default">Error!</response>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(CategoryResult))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CategoryResult>> GetCatogoryById(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound($"No category with ID: {id} is found.");

            return Ok(_mapper.Map<CategoryResult>(category));
        }


        // POST: /categories
        /// <summary>
        ///     Update entire category.
        /// </summary>
        /// <remarks>
        /// Schema:
        /// 
        ///     {
        ///       "name": "string"
        ///     }
        /// </remarks>
        /// <param name="categoryDTO"></param>
        /// <response code="204">Category added successfully.</response>
        /// <response code="400">Category name is already exist..</response>
        /// <response code="404">Category with ID provided not found.</response>
        /// <response code="default">Error!!</response>
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (await IsCategoryNameExistAsync(categoryDTO) is true)
                return BadRequest(@$"The Category with Name: '{categoryDTO.Name}' already exist!!.");

            var category = _mapper.Map<Category>(categoryDTO);
            var result = await _categoryRepository.AddAsync(category);

            return CreatedAtAction(nameof(GetCatogoryById), new { id = result.CategoryId }, _mapper.Map<CategoryResult>(result));
        }


        // PUT: /categories/{id:Guid}
        /// <summary>
        ///     Update entire category.
        /// </summary>
        /// <remarks>
        /// Schema:
        /// 
        ///     {
        ///       "name": "string"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="categoryDTO"></param>
        /// <response code="204">Category updated successfully.</response>
        /// <response code="400">Category name is already exist..</response>
        /// <response code="404">Category with ID provided not found.</response>
        /// <response code="default">Error!!</response>
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCategory(Guid id, CategoryDTO categoryDTO)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound($"No category with ID: {id} is found.");

            if (await IsCategoryNameExistAsync(categoryDTO) is true)
                return BadRequest(@$"The Category with Name: '{categoryDTO.Name}' already exist!!.");

            var result = await _categoryRepository.UpdateAsync(category.Update(categoryDTO));
            if (result is null)
                return StatusCode(500);

            return NoContent();
        }

        // GET: /categories/productsofcategorybyid/idCategory
        /// <summary>
        ///     Get products of category by ID.
        /// </summary>
        /// <param name="idCategory"></param>
        /// <param name="paging"></param>
        /// <param name="sorting"></param>
        /// <param name="searching"></param>
        /// <response code="200">Return all products of category by ID provided.</response>
        /// <response code="404">Category not found or Category with no products.</response>
        /// <response code="default">Error!!</response>
        [HttpGet("productsofcategorybyid/{idCategory:Guid}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(PagingResult<ProductResult>))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PagingResult<ProductResult>>> GetProductsOfCategoryById
            (
                Guid idCategory,
                [FromQuery] PagingDTO paging,
                [FromQuery] SortingDTO sorting,
                [FromQuery] ProductSearchingDTO searching
            )
        {
            var category = await _categoryRepository.GetByIdAsync(idCategory);
            if (category is null)
                return NotFound($"Category with ID: {idCategory} is not found.");

            var productsOfCategory = (await _productRepository.GetAllAsync())
                                            .Where(p => p.CategoryId == idCategory);

            if (productsOfCategory.Count() == 0)
                return NotFound("No Element Found !!..");

            //// Searching
            productsOfCategory = SeaechForProducts(productsOfCategory, searching);

            //// Sorting
            productsOfCategory = SortingProducts(productsOfCategory, sorting);

            //// Paging
            var pagingResult = PagingProducts(productsOfCategory, paging, _mapper);

            var outOfRange = (paging.Page - 1) * paging.Size > pagingResult.Paging.TotalRows;
            if (outOfRange is true)
                return BadRequest("Out of Range !!..");

            return Ok(pagingResult);
        }

        #endregion


        #region Private fields

        private async Task<bool> IsCategoryNameExistAsync(CategoryDTO categoryDTO)
            => (await _categoryRepository.GetAllAsync())
                                         .Where(c => c.Name.ToLower().Equals(categoryDTO.Name.ToLower()))
                                         .Any();

        #endregion

    }
}
