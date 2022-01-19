using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Interfaces;

public interface IUserService
{
    public User GetUser(int id);
    public IEnumerable<User> GetAllUsers();
}