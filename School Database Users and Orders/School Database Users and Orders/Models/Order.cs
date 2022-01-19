using System.ComponentModel.DataAnnotations;

namespace School_Database_Users_and_Orders.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public int TotalPrice { get; set; }
    public int AddressId { get; set; }
    public string AddressName { get; set; } = null!;
    public string AddressLine { get; set; } = null!;
    public int PostalNumber { get; set; }
    public string PostalPlace { get; set; } = null!;
    public string Country { get; set; } = null!;
    public DateTime OrderTimestamp { get; set; }
    public User User { get; set; } = null!;
}