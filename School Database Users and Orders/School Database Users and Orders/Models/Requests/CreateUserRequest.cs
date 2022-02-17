namespace School_Database_Users_and_Orders.Models.Requests;

public class CreateUserRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int PhoneNumber { get; set; }
    public string Pfp { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int AccessLevel { get; set; }
}