namespace School_Database_Users_and_Orders.Models;

// A Model describes a datatype. For example the Country is a string.
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Credentials Credentials{ get; set; } = null!;
    public string Email { get; set; } = null!;
    public int Number { get; set; }
    public string Pfp { get; set; } = null!;
}