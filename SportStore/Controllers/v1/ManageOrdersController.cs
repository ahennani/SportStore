namespace SportStore.Controllers.v1;

[Route("/orders")]
[ApiController]
[ApiVersion("1.0")]
public class ManageOrdersController : ControllerBase
{
    private readonly IOrderManager _orderManager;
    private readonly IStoreRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public ManageOrdersController(IOrderManager orderManager, IStoreRepository<Product> productRepository, IMapper mapper)
    {
        _orderManager = orderManager;
        _productRepository = productRepository;
        _mapper = mapper;
    }


    // GET: /orders
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<OrderReult>))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(string))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<OrderReult>>> GetAllOrdersAsync()
    {
        var orders = await _orderManager.GetAllAsync();
        if (orders.Any() is false)
            return NotFound("No Order has been found !!");

        var orderResults = _mapper.Map<IEnumerable<OrderReult>>(orders);

        return Ok(orderResults);
    }


    // GET: /orders/{id:Guid}
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(OrderReult))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(string))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderReult>> GetOrderByIdAsync(Guid id)
    {
        var order = await _orderManager.GetByIdAsync(id);
        if (order is null)
            return NotFound("No Order has been found !!");

        var orderResult = _mapper.Map<OrderReult>(order);
        await GetOrderProducts(order, orderResult);

        return Ok(orderResult);
    }


    // POST: /orders/addproductstoorder/{id:Guid}
    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(OrderReult))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderReult>> CreateOrder(params OrderProductsDTO[] orderProductsDTOs)
    {
        if (orderProductsDTOs.Length == 0)
            return BadRequest("No product have been provided.");

        var products = new List<Product>();
        foreach (var orderProduct in orderProductsDTOs)
        {
            var prodact = await _productRepository.GetByIdAsync(orderProduct.Id);
            if (prodact is null)
                return NotFound($"No product with ID: {orderProduct.Id} is found.");

            if (prodact.Quantity < orderProduct.Quantity)
                return BadRequest($"Quantity of Product with ID: {orderProduct.Id} does not exist in the stock.");

            products.Add(prodact);
        }

        //// TODO: Uncomment when App Require Auth
        //var UserId = User.Claims.SingleOrDefault(c => c.Type == "UserId").Value;
        var UserId = "cd707f02-c08e-47ba-a57f-47921ea64b08";

        var order = new Order()
        {
            OrderId = Guid.NewGuid(),
            UserId = Guid.Parse(UserId),
            Products = products
        };

        var entity = await _orderManager.AddAsync(order);
        if (entity is null)
            return StatusCode(500);

        var orderResult = _mapper.Map<OrderReult>(order);
        //await GetOrderProducts(order, orderResult);

        return Created(nameof(GetOrderByIdAsync), orderResult);
    }


    // DELETE: /orders
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(string))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderReult>> DeleteOrder(Guid id)
    {
        var order = await _orderManager.GetByIdAsync(id);
        if (order is null)
            return NotFound("No Order has been found !!");

        var entity = await _orderManager.DeleteAsync(id);
        if (entity is null)
            return StatusCode(500);

        return NoContent();
    }

    // POST: /orders/multidelete/{ids}
    [HttpPost("multidelete")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(string))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderReult>> DeleteOrderRangeAsync([FromQuery] params Guid[] ids)
    {
        if (ids.Length == 0)
            return BadRequest("No product have been provided.");

        //// TODO: Delete/SetNull OrderId Column before deleting.
        var orders = new List<Order>();
        foreach (var id in ids)
        {
            var order = await _orderManager.GetByIdAsync(id);
            if (order is null)
                return NotFound($"No Order with ID: {id} is found.");

            orders.Add(order);
        }

        var result = await _orderManager.DeleteRangeAsync(orders);
        if (result is false)
            return StatusCode(500);

        return Ok();
    }


    // POST: /orders/addproductstoorder/{id:Guid}
    [HttpPost("addproductstoorder/{id:Guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(OrderReult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(string))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderReult>> AddProductsToOrder(Guid id, params OrderProductsDTO[] orderProductsDTOs)
    {
        var order = await _orderManager.GetByIdAsync(id);
        if (order is null)
            return NotFound("No Order has been found !!");

        if (orderProductsDTOs.Length == 0)
            return BadRequest("No product have been provided.");


        var products = new List<Product>();
        foreach (var orderProduct in orderProductsDTOs)
        {
            var prodact = await _productRepository.GetByIdAsync(orderProduct.Id);
            if (prodact is null)
                return NotFound($"No product with ID: {orderProduct.Id} is found.");

            if (prodact.Quantity < orderProduct.Quantity)
                return BadRequest($"Quantity of Product with ID: {orderProduct.Id} does not exist in the stock.");

            products.Add(prodact);
        }

        var result = await _orderManager.AddProductsToOrder(order, products.ToArray());

        var orderResult = _mapper.Map<OrderReult>(order);
        await GetOrderProducts(order, orderResult);

        return Created(nameof(GetOrderByIdAsync), orderResult);
    }



    #region Private fields

    private async Task GetOrderProducts(Order order, OrderReult orderResult)
    {
        var orderProducts = await _orderManager.GetOrderProducts(order);
        if (orderProducts is not null)
        {
            orderResult.Products = _mapper.Map<IEnumerable<ProductResult>>(orderProducts);
        }
    }

    #endregion

}

//var product1 = await _context.Products.FindAsync(Guid.Parse("2dd21234-5d84-4b11-2a6f-08d9a63e07dc"));
//var product2 = await _context.Products.FindAsync(Guid.Parse("b84727a2-77b7-460e-fd7f-08d999a3734c"));
//var product3 = await _context.Products.FindAsync(new Guid("31454343-36ac-4e67-f67a-08d999a230af"));

//var order = new Order();
//order.OrderId = Guid.NewGuid();
//order.OrderDate = DateTime.Now;
//order.UserId = Guid.Parse("cd707f02-c08e-47ba-a57f-47921ea64b08");
//order.Products.AddRange(new List<Product> { product1, product2, product3 });

//_context.Orders.Add(order);
//var result = await _context.SaveChangesAsync();