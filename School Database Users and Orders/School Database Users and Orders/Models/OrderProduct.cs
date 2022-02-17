namespace School_Database_Users_and_Orders.Models;

// A Model describes a datatype. For example the Country is a string.
public class OrderProduct
{
    public int OrderId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}