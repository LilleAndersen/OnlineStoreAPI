using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Interfaces;

// The interface acts as a link between the service and controller, it describes the service to the controller
public interface IUserService
{
    public User GetUser(string token);
    public IEnumerable<User> GetAllUsers();
    public bool CreateUser(string firstName, string lastName, string username, string email, int phoneNumber, string pfp, string password, int accessLevel);
    public bool DeleteUser(string username);
}