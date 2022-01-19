namespace School_Database_Users_and_Orders.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username{ get; set; } = null!;
    public int Number { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public int AccessLevel { get; set; }
    
}