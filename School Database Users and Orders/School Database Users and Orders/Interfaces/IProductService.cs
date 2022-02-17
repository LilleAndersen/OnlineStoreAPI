using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Interfaces;

// The interface acts as a link between the service and controller, it describes the service to the controller
public interface IProductService
{
    public Product GetProduct(int id);
    public IEnumerable<Product> GetAllProducts();
}