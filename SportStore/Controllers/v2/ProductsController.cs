namespace SportStore.Controllers.v2;

[ApiVersion("2.0")]
[Route("/products")]
[ApiController]
[SwaggerTag("Create, read, update and delete Products.")]
public class ProductsController : ControllerBase
{
    private readonly IStoreRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public ProductsController(IStoreRepository<Product> productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get All Products With Pagination Options
    /// </summary>
    /// <param name="paging"></param>
    /// <param name="sorting"></param>
    /// <param name="searching"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<PagingResult<ProductDTO>>> GetAllProducts([FromQuery] PagingDTO paging,
                                                   [FromQuery] SortingDTO sorting,
                                                   [FromQuery] ProductSearchingDTO searching)
    {

        var products = await _productRepository.GetAllAsync();

        //// Searching
        products = SeaechForProducts(products, searching);

        //// Sorting
        products = SortingProducts(products, sorting);

        //// Paging
        var pagingResult = PagingProducts(products, paging);

        if (pagingResult.Paging.TotalRows is 0)
        { return NotFound("No Element Found !!.."); }

        var outOfRange = (paging.Page - 1) * paging.Size > pagingResult.Paging.TotalRows;
        if (outOfRange is true)
        { return NotFound("Out of Range !!.."); }

        return Ok(pagingResult);
    }

    #region Private Fields

    private IQueryable<Product> SeaechForProducts(IQueryable<Product> products, ProductSearchingDTO searching)
    {
        if (!String.IsNullOrEmpty(searching?.SearchTerm))
        {
            products = products.Where
                (p =>
                    p.Name.ToLower().Contains(searching.SearchTerm.ToLower())
                    ||
                    p.SKU.ToLower().Contains(searching.SearchTerm.ToLower())
                );
        }

        if (searching?.MinPrice is not null)
        {
            products = products.Where(p => p.Price >= searching.MinPrice.Value);
        }

        if (searching?.MaxPrice is not null)
        {
            products = products.Where(p => p.Price <= searching.MaxPrice.Value);
        }

        if (!String.IsNullOrEmpty(searching?.SKU))
        {
            products = products.Where(p => p.SKU.ToLower().Contains(searching.SKU.ToLower()));
        }

        if (!String.IsNullOrEmpty(searching?.Name))
        {
            products = products.Where(p => p.Name.ToLower().Contains(searching.Name.ToLower()));
        }

        return products;
    }

    private IQueryable<Product> SortingProducts(IQueryable<Product> products, SortingDTO sorting)
    {
        if (!String.IsNullOrEmpty(sorting?.SortBy))
        {
            var property = typeof(Product)
                            .GetProperty(sorting.SortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property is not null)
            {
                products = products.OrderByCustome<Product>(sorting.SortOrder, sorting.SortBy);
            }
        }

        return products;
    }

    private PagingResult<ProductResult> PagingProducts(IQueryable<Product> products, PagingDTO paging)
    {
        var productsResult = _mapper.Map<IEnumerable<ProductResult>>(products);

        return new PagingResult<ProductResult>(productsResult, paging);
    }

    #endregion
}
