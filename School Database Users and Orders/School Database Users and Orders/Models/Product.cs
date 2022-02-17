namespace School_Database_Users_and_Orders.Models;

// A Model describes a datatype. For example the Country is a string.
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public float Price { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
}