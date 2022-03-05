using System.Security.Cryptography;
using System.Text;
using MySqlConnector;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace School_Database_Users_and_Orders.Services;

// Does most of the grunt work, connects to the api and reads from it, thereof returns the request to the requester.
public class UserService : IUserService
{
    private static readonly Random Random = new Random();

    // Produces and random token for credentials for each user
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdhijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
    
    // Function to decode a byte array to a string
    private static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        var sOutput = new StringBuilder(arrInput.Length);
        for (i = 0; i < arrInput.Length; i++) sOutput.Append(arrInput[i].ToString("X2"));
        return sOutput.ToString();
    }
    
    // Gets a user based on the credentials token
    public User GetUser(string token)
    {
        var user = new User();

        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "select * from user_order_database.user, user_order_database.credentials where token = @token and credentials_username = username"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);

        command.Parameters.AddWithValue("@token", token);

        connection.Open();

        using var reader = command.ExecuteReader();
        
        // Reads what the database returns after the SQL statement
        // Puts all relevant information in variables in JSON readable format
        while (reader.Read())
        {
            user.Id = (int) reader["uid"];
            user.FirstName = (string) reader["first_name"];
            user.LastName = (string) reader["last_name"];
            user.Email = (string) reader["email"];
            user.Number = (int) reader["phone_number"];
            user.Pfp = (string) reader["pfp"];
            user.Credentials = new Credentials
            {
                Username = (string) reader["credentials_username"],
                Password = (string) reader["password"],
                Token = (string) reader["token"],
                AccessLevel = (int) reader["access_level"]
            };
        }

        return user;
    }

    // Gets all users
    public IEnumerable<User> GetAllUsers()
    {
        var list = new List<User>();

        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "select * from user_order_database.user, user_order_database.credentials where credentials_username = username"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);
        
        connection.Open();

        using var reader = command.ExecuteReader();
        
        // Reads what the database returns after the SQL statement
        // Puts all relevant information in variables in JSON readable format
        while (reader.Read())
        {
            list.Add(new User
            {
                Id = (int) reader["uid"],
                FirstName = (string) reader["first_name"],
                LastName = (string) reader["last_name"],
                Email = (string) reader["email"],
                Number = (int) reader["phone_number"],
                Pfp = (string) reader["pfp"],
                Credentials = new Credentials
                {
                    Username = (string) reader["credentials_username"],
                    Password = (string) reader["password"],
                    Token = (string) reader["token"],
                    AccessLevel = (int) reader["access_level"]
                }
            });
        }

        return list;
    }

    // Creates a user based on appropriate informasjon
    public bool CreateUser(string firstName, string lastName, string username, string email, int phoneNumber, string pfp, string password, int accessLevel)
    {

        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "insert into user_order_database.user (first_name, last_name, email, credentials_username, phone_number, pfp) values (@firstName, @lastName, @email, @username, @phoneNumber, @pfp)"; // The SQL statement itself, what gets executed in the database
        const string credentialscommandString = "insert into user_order_database.credentials (username, password, token, access_level) values (@username, @password, @token, @accessLevel)"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);
        var credentialsCommand = new MySqlCommand(credentialscommandString, connection);

        // Encodes and hashes the password for comparison to the database
        var passBytes = Encoding.UTF8.GetBytes(password);
        var passHash = SHA256.Create().ComputeHash(passBytes);
        
        command.Parameters.AddWithValue("@firstName", firstName);
        command.Parameters.AddWithValue("@lastName", lastName);
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
        command.Parameters.AddWithValue("@pfp", pfp);
        credentialsCommand.Parameters.AddWithValue("@username", username);
        credentialsCommand.Parameters.AddWithValue("@password", ByteArrayToString(passHash));
        credentialsCommand.Parameters.AddWithValue("@token", RandomString(64));
        credentialsCommand.Parameters.AddWithValue("@accessLevel", accessLevel);

         try
        {
            connection.Open();
            credentialsCommand.ExecuteNonQuery();
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        } 

        return true;
    }

    // Deletes a user based on the username of the user
    public bool DeleteUser(string username)
    {
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "delete from user_order_database.user where credentials_username = @username;delete from user_order_database.credentials where username = @username"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);
        
        command.Parameters.AddWithValue("@username", username);
        
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

    public bool UpdateUser(string payloadToken, string? payloadFirstName, string? payloadLastName, string? payloadEmail, int? payloadNumber, string? payloadPfp)
    {
        var user = GetUser(payloadToken);

        payloadFirstName ??= user.FirstName;
        payloadLastName ??= user.LastName;
        payloadEmail ??= user.Email;
        payloadNumber ??= user.Number;
        payloadPfp ??= user.Pfp;

        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "update user_order_database.user set email = @email , phone_number = @number , first_name = @firstname , last_name = @lastname , pfp = @pfp where credentials_username = @username"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);
        
        command.Parameters.AddWithValue("@firstName", payloadFirstName);
        command.Parameters.AddWithValue("@lastName", payloadLastName);
        command.Parameters.AddWithValue("@username", user.Credentials.Username);
        command.Parameters.AddWithValue("@email", payloadEmail);
        command.Parameters.AddWithValue("@number", payloadNumber);
        command.Parameters.AddWithValue("@pfp", payloadPfp);
        
        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}