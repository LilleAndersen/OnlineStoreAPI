namespace School_Database_Users_and_Orders.Models;

// A Model describes a datatype. For example the Country is a string.
public class PostalNumber
{
    public int Number { get; set; }
    public string Place { get; set; } = null!;
}