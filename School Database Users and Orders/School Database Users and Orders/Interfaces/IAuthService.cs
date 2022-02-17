namespace School_Database_Users_and_Orders.Interfaces;

// The interface acts as a link between the service and controller, it describes the service to the controller
public interface IAuthService
{
    public string VerifyCredentials(string user, string pass);
    public bool UpdatePassword(string user, string pass, string newPass);
}