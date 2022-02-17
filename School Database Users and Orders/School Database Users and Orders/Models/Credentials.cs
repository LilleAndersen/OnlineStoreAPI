namespace School_Database_Users_and_Orders.Models;

// A Model describes a datatype. For example the Country is a string.
public class Credentials
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Token { get; set; } = null!;
    public int AccessLevel { get; set; }
}