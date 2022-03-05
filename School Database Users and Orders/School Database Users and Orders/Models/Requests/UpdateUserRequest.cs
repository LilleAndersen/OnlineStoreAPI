namespace School_Database_Users_and_Orders.Models;

// A Model describes a datatype. For example the Country is a string.
public class UpdateUserRequest
{
    public string Token { get; set; } = null!;
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public int? Number { get; set; }
    public string? Pfp { get; set; } = null!;
}