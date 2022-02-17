namespace School_Database_Users_and_Orders.Models.Requests;

public class UpdateCredentialsRequest
{
    public string User { get; set; } = null!;
    public string Pass { get; set; } = null!;
    public string NewPass { get; set; } = null!;
}