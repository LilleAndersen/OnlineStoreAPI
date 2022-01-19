using Microsoft.AspNetCore.Mvc;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Controllers;

[ApiController]
[Route("users")]
public class UserController : Controller
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public User GetUser(int id)
    {
        return _userService.GetUser(id);
    }

    [HttpGet("all")]
    public IEnumerable<User> GetAllUsers()
    {
        return _userService.GetAllUsers();
    }
}