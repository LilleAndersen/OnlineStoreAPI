using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Controllers;

// Tells file that is an API controller (it controls the api)
[ApiController]
[Route("orders")] // Defines the route which the controller takes control of. For example the url "localhost/orders" would be controlled by this controller
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // HTTP Get Request which gets the an order specified by the id of the order
    [HttpGet]
    public Order GetOrder(int id)
    {
        return _orderService.GetOrder(id);
    }

    // HTTP Get Request which all orders associated with a user
    [HttpGet("user")]
    public IEnumerable<Order> GetUserOrders(int id)
    {
        return _orderService.GetUserOrders(id);
    }

    // HTTP Get Request which makes a new order based on user id, address id and total price of order
    [HttpPost("new")]
    public bool CreateOrder(int userId, int addressId, float totalPrice)
    {
        return _orderService.CreateOrder(userId, addressId, totalPrice);
    }

    // HTTP Post Request which adds a specified product to order
    [HttpPost("link")]
    public bool AddProductToOrder(int orderId, int productId, int quantity)
    {
        return _orderService.AddProductToOrder(orderId, productId, quantity);
    }

    // HTTP Get Request which updates the status of the order
    [HttpPost("update")]
    public bool UpdateOrderStatus(string status, int id)
    {
        return _orderService.UpdateOrderStatus(status, id);
    }
}