using System.Security.Cryptography;
using System.Text;
using MySqlConnector;
using School_Database_Users_and_Orders.Interfaces;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace School_Database_Users_and_Orders.Services;


// Does most of the grunt work, connects to the api and reads from it, thereof returns the request to the requester.
public class AuthService: IAuthService
{
    
    // Function to decode a byte array to a string
    private static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        var sOutput = new StringBuilder(arrInput.Length);
        for (i = 0; i < arrInput.Length; i++) sOutput.Append(arrInput[i].ToString("X2"));
        return sOutput.ToString();
    }
    
    // Verifies the credentials with the database
    public string VerifyCredentials(string user, string pass)
    {
        var token = "";
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "select token from user_order_database.credentials where username = @user and password = @pass"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);

        // Encodes and hashes the password for comparison to the database
        var passBytes = Encoding.UTF8.GetBytes(pass);
        var passHash = SHA256.Create().ComputeHash(passBytes);
        
        // Variables for the SQL statement, you put @user in the SQL statement and define it outside the statement
        command.Parameters.AddWithValue("@user", user);
        command.Parameters.AddWithValue("@pass", ByteArrayToString(passHash));

        // Opens the connection to the database
        connection.Open();
        
        // Reads what the database returns after the SQL statement
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            token = (string) reader[0];
        }

        // Returns the token to the requester
        return token;
    }

    // Updates the password of specified user
    public bool UpdatePassword(string user, string pass, string newPass)
    {
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "update credentials set password = @newPass where username = @user and password = @pass"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);

        // Encodes and hashes the password for comparison to the database
        var passBytes = Encoding.UTF8.GetBytes(pass);
        var passHash = SHA256.Create().ComputeHash(passBytes);
        
        // Encodes and hashes the password for comparison to the database
        var newPassBytes = Encoding.UTF8.GetBytes(newPass);
        var newPassHash = SHA256.Create().ComputeHash(newPassBytes);

        // Variables for the SQL statement, you put @user in the SQL statement and define it outside the statement
        command.Parameters.AddWithValue("@user", user);
        command.Parameters.AddWithValue("@pass", ByteArrayToString(passHash));
        command.Parameters.AddWithValue("@newPass", ByteArrayToString(newPassHash));
        
        // Tries the connection and logs the error if any are caught.
        try
        {
            connection.Open();
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        } 

        return true;
    }
}