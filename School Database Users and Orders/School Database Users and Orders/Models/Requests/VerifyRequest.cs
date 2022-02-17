namespace School_Database_Users_and_Orders.Models.Requests;

public class VerifyRequest
{
    public string User { get; set; } = null!;
    public string Pass { get; set; } = null!;
}