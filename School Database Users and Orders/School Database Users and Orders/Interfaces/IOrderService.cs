using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Interfaces;

// The interface acts as a link between the service and controller, it describes the service to the controller
public interface IOrderService
{
    public Order GetOrder(int id);
    public IEnumerable<Order> GetUserOrders(int id);
    public bool CreateOrder(int id, int addressId, float totalPrice);
    public bool AddProductToOrder(int orderId, int productId, int quantity);
    public bool UpdateOrderStatus(string status, int id);

    public int CreateAddress(string addressName, string addressLine, string postalNumber, string country);
}