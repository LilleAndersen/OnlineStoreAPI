using Microsoft.AspNetCore.Mvc;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Controllers;

// Tells file that is an API controller (it controls the api)
[ApiController]
[Route("products")] // Defines the route which the controller takes control of. For example the url "localhost/products" would be controlled by this controller
public class ProductController : Controller
{

    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // HTTP Get Request which gets a product based on the product id
    [HttpGet]
    public Product GetProduct(int id)
    {
        return _productService.GetProduct(id);
    }
    
    // HTTP Get Request which gets all orders
    [HttpGet("all")]
    public IEnumerable<Product> GetAllProducts()
    {
        return _productService.GetAllProducts();
    }
}