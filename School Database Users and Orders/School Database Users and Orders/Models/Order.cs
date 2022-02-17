using System.ComponentModel.DataAnnotations;

namespace School_Database_Users_and_Orders.Models;

// A Model describes a datatype. For example the Country is a string.
public class Order
{
    public int Id { get; set; }
    public User User { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public IEnumerable<OrderProduct> Products { get; set; } = null!;
    public float TotalPrice { get; set; }
    public string Status { get; set; } = null!;
    public DateTime OrderTimestamp { get; set; }
}