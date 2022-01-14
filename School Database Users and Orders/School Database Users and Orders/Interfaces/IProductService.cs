using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Interfaces;

public interface IProductService
{
    public Product GetProduct(int id);
    public IEnumerable<Product> GetAllProducts();
}