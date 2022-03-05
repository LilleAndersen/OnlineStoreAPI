using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;
using School_Database_Users_and_Orders.Models.Requests;

namespace School_Database_Users_and_Orders.Controllers;

// Tells file that is an API controller (it controls the api)
[ApiController]
[Route("users")] // Defines the route which the controller takes control of. For example the url "localhost/users" would be controlled by this controller
public class UserController : Controller
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // HTTP Get Request which gets a user based on the credentials token
    [HttpGet]
    public User GetUser([FromHeader]string token)
    {
        return _userService.GetUser(token);
    }

    // HTTP Get Request which gets all users
    [HttpGet("all")]
    public IEnumerable<User> GetAllUsers()
    {
        return _userService.GetAllUsers();
    }

    // HTTP Post Request which creates a user based on appropriate information
    [HttpPost]
    public bool CreateUser([FromBody] CreateUserRequest payload)
    {
        return _userService.CreateUser(payload.FirstName, payload.LastName, payload.Username, payload.Email, payload.PhoneNumber, payload.Pfp, payload.Password, payload.AccessLevel);
    }

    // HTTP Delete Request which deletes a user based on its username
    [HttpDelete]
    public bool DeleteUser([FromHeader]string username)
    {
        return _userService.DeleteUser(username);
    }

    [HttpPost("update")]
    public bool UpdateUser([FromBody] UpdateUserRequest payload)
    {
        return _userService.UpdateUser(payload.Token, payload.FirstName, payload.LastName, payload.Email,
            payload.Number, payload.Pfp);
    }
}