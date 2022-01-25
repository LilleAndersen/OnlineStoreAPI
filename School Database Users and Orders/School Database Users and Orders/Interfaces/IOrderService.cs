using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Interfaces;

public interface IOrderService
{
    public Order GetOrder(int id);
}