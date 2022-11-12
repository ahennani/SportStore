namespace SportStore.Helpers;

public class ProductFiltring
{
    public static IQueryable<Product> SeaechForProducts(IQueryable<Product> products, ProductSearchingDTO searching)
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

    public static IQueryable<Product> SortingProducts(IQueryable<Product> products, SortingDTO sorting)
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

    public static PagingResult<ProductResult> PagingProducts(IQueryable<Product> products, PagingDTO paging, IMapper mapper)
    {
        var productsResult = mapper.Map<IEnumerable<ProductResult>>(products);

        return new PagingResult<ProductResult>(productsResult, paging);
    }
}
