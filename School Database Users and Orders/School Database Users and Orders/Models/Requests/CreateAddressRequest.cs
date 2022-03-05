namespace School_Database_Users_and_Orders.Models.Requests;

public class CreateAddressRequest
{
    public string AddressName { get; set; } = null!;
    public string AddressLine { get; set; } = null!;
    public string PostalNumber { get; set; } = null!;
    public string Country { get; set; } = null!;
}
