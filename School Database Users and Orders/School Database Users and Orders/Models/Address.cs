namespace School_Database_Users_and_Orders.Models;

// A Model describes a datatype. For example the Country is a string.
public class Address
{
    public int AddressId { get; set; }
    public string AddressName { get; set; } = null!;
    public string AddressLine { get; set; } = null!;
    public PostalNumber PostalNumber { get; set; } = null!;
    public string Country { get; set; } = null!;
}