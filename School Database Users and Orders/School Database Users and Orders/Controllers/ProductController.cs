using Microsoft.AspNetCore.Mvc;
using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Controllers;

[ApiController]
[Route("product")]
public class ProductController : Controller
{
    [HttpGet]
    public IEnumerable<Product> GetAllProducts()
    {
        return new List<Product>();
    }
}