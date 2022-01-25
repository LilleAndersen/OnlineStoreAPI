using System.ComponentModel.DataAnnotations;

namespace School_Database_Users_and_Orders.Models;

public class Order
{
    public int Id { get; set; }
    public User User { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public float TotalPrice { get; set; }
    public DateTime OrderTimestamp { get; set; }
}