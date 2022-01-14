namespace School_Database_Users_and_Orders.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public float Price { get; set; }
    public int Stock { get; set; }
    public string ImageUrl { get; set; } = null!;
}