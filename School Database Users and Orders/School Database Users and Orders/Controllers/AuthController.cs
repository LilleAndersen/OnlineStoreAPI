using Microsoft.AspNetCore.Mvc;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models.Requests;

namespace School_Database_Users_and_Orders.Controllers;

// Tells file that is an API controller (it controls the api)
[ApiController]
[Route("auth")] // Defines the route which the controller takes control of. For example the url "localhost/auth" would be controlled by this controller
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // HTTP Post Request which verifies my credentials according to my table
    [HttpPost("verify")]
    public string VerifyCredentials([FromBody] VerifyRequest payload)
    {
        return _authService.VerifyCredentials(payload.User, payload.Pass);
    }

    // HTTP Post Request which update the password on specified user
    [HttpPost("update")]
    public bool UpdatePassword([FromBody] UpdateCredentialsRequest payload)
    {
        return _authService.UpdatePassword(payload.User, payload.Pass, payload.NewPass);
    }
}