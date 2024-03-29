﻿using static SportStore.Helpers.ProductFiltring;

namespace SportStore.Controllers.v1;

[ApiController]
[Route("products")]
[ApiVersion("1.0")]
[SwaggerTag("Create, read, update and delete Products.")]
[Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
public class ProductsController : ControllerBase
{
    private readonly IStoreRepository<Product> _productRepository;
    private readonly IStoreRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController
        (
            IStoreRepository<Product> productRepository,
            IStoreRepository<Category> categoryRepository,
            IMapper mapper,
            ILogger<ProductsController> logger
        )
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }


    #region Endpoints


    // GET: /products
    /// <summary>
    /// Get All Products With Pagination Options.
    /// </summary>
    /// <param name="paging"></param>
    /// <param name="sorting"></param>
    /// <param name="searching"></param> 
    /// <response code="200">Return all founded products.</response>
    /// <response code="404">There are no products with elements provided!.</response> 
    /// <response code="default">Error!!</response>
    [HttpGet()]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(PagingResult<ProductDTO>))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<PagingResult<ProductResult>>> GetAllProducts
        (
            [FromQuery] PagingDTO paging,
            [FromQuery] SortingDTO sorting,
            [FromQuery] ProductSearchingDTO searching
        )
    {
        var products = await _productRepository.GetAllAsync();

        //// Searching
        products = SeaechForProducts(products, searching);

        //// Sorting
        products = SortingProducts(products, sorting);

        //// Paging
        var pagingResult = PagingProducts(products, paging, _mapper);

        if (pagingResult.Paging.TotalRows is 0)
        { return NotFound("No Element Found !!.."); }

        var outOfRange = (paging.Page - 1) * paging.Size > pagingResult.Paging.TotalRows;
        if (outOfRange is true)
        { return NotFound("Out of Range !!.."); }

        return Ok(pagingResult);
    }


    // GET: /products/availableproducts
    /// <summary>
    ///     Get available (quantity > 0) products with pagination options.
    /// </summary>
    /// <param name="paging"></param>
    /// <param name="sorting"></param>
    /// <param name="searching"></param> 
    /// <response code="200">User logged in successfully.</response>
    /// <response code="404">There are no products with elements provided!.</response> 
    /// <response code="default">Error !.</response> 
    [HttpGet("availableproducts")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(PagingResult<ProductDTO>))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<PagingResult<ProductResult>>> GetAvailableProducts
        (
            [FromQuery] PagingDTO paging,
            [FromQuery] SortingDTO sorting,
            [FromQuery] ProductSearchingDTO searching
        )
    {
        var availableProducts = ((IEnumerable<Product>)(await _productRepository.GetAllAsync()))
                                    .Where(p => p.IsAvalibale == true);

        var products = availableProducts.AsQueryable();

        //// Searching
        products = SeaechForProducts(products, searching);

        //// Sorting
        products = SortingProducts(products, sorting);

        //// Paging
        var pagingResult = PagingProducts(products, paging, _mapper);

        if (pagingResult.Paging.TotalRows is 0)
            return NotFound("No Element Found !!..");

        var outOfRange = (paging.Page - 1) * paging.Size > pagingResult.Paging.TotalRows;
        if (outOfRange is true)
            return BadRequest("Out of Range !!..");

        return Ok(pagingResult);
    }


    // GET: /products/{id}
    /// <summary>
    ///     Get a product by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Returns the product with id provided</response>
    /// <response code="400">The product details is invalid.</response>
    /// <response code="404">The product not found</response>
    /// <response code="default">Error!.</response>
    [HttpGet("{id:Guid}", Name = nameof(GetProductById))]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ProductDTO))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProductResult>> GetProductById(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            return NotFound("No Element With The Id Provided !!");

        var productResult = _mapper.Map<ProductResult>(product);

        return Ok(productResult);
    }


    // POST: /products
    /// <summary>
    ///  Create new product. 
    /// </summary>
    /// <remarks>
    /// Schema:
    /// 
    ///     {
    ///       "sku": "string",
    ///       "name": "string",
    ///       "descriptio": "string",
    ///       "price": 0,
    ///       "quantity": 0,
    ///       "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///     }
    /// </remarks>
    /// <response code="201"> Product created successfully!. </response>
    /// <response code="400"> The product details is invalid!.</response>
    /// <response code="401"> Unauthorized: Should login to call this endpoint!.</response>
    /// <response code="default"> Error!.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> CreateProduct(ProductDTO productDTO)
    {
        if ((await IsSKUExist(productDTO.SKU)) is true)
            return BadRequest(@$"The product with SKU: '{productDTO.SKU}' already exist!!.");

        var product = _mapper.Map<Product>(productDTO);

        var entity = await _productRepository.AddAsync(product);
        if (entity is null)
            return StatusCode(500);

        return CreatedAtAction(nameof(GetProductById), new { id = entity.ProductId }, productDTO);
    }


    // PUT: //products/{id:Guid}
    /// <summary>
    ///     Update whole product with specific ID
    /// </summary>
    /// <remarks>
    /// Schema:
    /// 
    ///     {
    ///       "sku": "string",
    ///       "name": "string",
    ///       "descriptio": "string",
    ///       "price": 0,
    ///       "quantity": 2147483647,
    ///       "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///     }
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="productDTO"></param>
    /// <response code="204">Product updated successfully.</response>
    /// <response code="400">The product details is invalid.</response>
    /// <response code="404">The product not found.</response>
    /// <response code="default">Error!.</response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> FullUpdateProductAsync([FromRoute] Guid id, [FromBody] ProductDTO productDTO)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            return NotFound("No product was found!!.");

        if (product.SKU != productDTO.SKU)
        {
            var isExist = await IsSKUExist(productDTO.SKU);

            if (isExist is true)
                return BadRequest(@$"The product with SKU: '{productDTO.SKU}' already exist!!.");
        }

        try
        {
            var entity = await _productRepository.UpdateAsync(product.Update(productDTO));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if ((await _productRepository.GetByIdAsync(id)) is null)
            {
                return NotFound();
            }

            _logger.LogError(ex, ex.Message);
        }

        return NoContent();
    }

    // PATCH: //products/{id:Guid}
    /// <summary>
    ///     Update product partially.
    /// </summary>
    /// <remarks>
    /// Simpe Schema
    ///     
    ///     [
    ///      {
    ///        "op": "replace",
    ///        "path": "sku",
    ///        "value": "test value"
    ///      }
    ///     ] 
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="productPatch"></param>
    /// <response code="204">Product updated successfully.</response>
    /// <response code="400">The product details is invalid.</response>
    /// <response code="404">The product not found.</response>
    /// <response code="default">Error!.</response>
    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> PartialUpdateProductAsync([FromRoute] Guid id, [FromBody] JsonPatchDocument<Product> productPatch)
    {
        foreach (var operation in productPatch.Operations)
        {
            var path = operation.path;
            if (path.Equals(nameof(Product.SKU), StringComparison.OrdinalIgnoreCase))
            {
                var value = operation.value.ToString();
                var isExist = await IsSKUExist(value); ;

                if (isExist is true)
                    return BadRequest(@$"The product with SKU: '{value}' already exist!!.");
            }
        }

        var product = await _productRepository.GetByIdAsync(id);

        productPatch.ApplyTo(product);

        try
        {
            var entity = await _productRepository.UpdateAsync(product);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if ((await _productRepository.GetByIdAsync(id)) is null)
            {
                return NotFound();
            }

            _logger.LogError(ex, ex.Message);
        }

        return NoContent();
    }


    // DELETE: /products/{id:Guid}
    /// <summary>
    /// Delete a single product by ID
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Product deleted successfully.</response>
    /// <response code="404">The product not found.</response>
    /// <response code="default">Error!.</response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProductResult>> DeleteProductAsync([FromRoute] Guid id)
    {
        var product = await _productRepository.DeleteAsync(id);
        if (product is null)
            return NotFound();

        var result = _mapper.Map<ProductResult>(product);

        return Ok(result);
    }


    // POST: /products/deleteproducts/{ids} === //products/deleteproducts?ids=1&ids=2&ids=3...
    /// <summary>
    /// Delete multiple products by IDs.
    /// </summary>
    /// <param name="ids"></param>
    /// <responce code="200">Products deleted successfully.</responce>
    /// <responce code="404">A product does not exist with prevant deleting the exsiting ones.</responce>
    /// <responce code="default">Error!!</responce>
    /// <returns>List of deleted products.</returns>
    [HttpPost("multidelete")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<ProductResult>))]
    public async Task<ActionResult<List<ProductResult>>> DeleteMultiple([FromQuery] Guid[] ids)
    {
        if (ids.Length is 0)
            return BadRequest();

        var products = new List<Product>();
        foreach (var id in ids)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return NotFound($"Product with ID: {id} is not found!!.");

            products.Add(product);
        }

        var result = await _productRepository.DeleteRangeAsync(products);
        if (result is false)
            return StatusCode(500);

        return Ok(_mapper.Map<List<ProductResult>>(products));
    }

    #endregion

    #region Private Fields

    private async Task<bool> IsSKUExist(string sku)
    {
        return (await _productRepository.GetAllAsync()).AsEnumerable()
                                .Any(p => p.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase));
    }

    #endregion

}
