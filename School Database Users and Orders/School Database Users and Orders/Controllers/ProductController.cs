using Microsoft.AspNetCore.Mvc;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Controllers;

[ApiController]
[Route("products")]
public class ProductController : Controller
{

    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public Product GetProduct(int id)
    {
        return _productService.GetProduct(id);
    }
    
    [HttpGet("all")]
    public IEnumerable<Product> GetAllProducts()
    {
        return _productService.GetAllProducts();
    }
}