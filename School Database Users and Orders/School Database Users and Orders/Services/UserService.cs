using MySqlConnector;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace School_Database_Users_and_Orders.Services;

public class UserService : IUserService
{
    public User GetUser(int id)
    {
        var user = new User();

        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "select * from user_order_database.user, user_order_database.credentials where uid = @id and credentials_username = username";
        var command = new MySqlCommand(commandString, connection);

        command.Parameters.AddWithValue("@id", id);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            user.Id = (int) reader["uid"];
            user.FirstName = (string) reader["first_name"];
            user.LastName = (string) reader["last_name"];
            user.Email = (string) reader["email"];
            user.Number = (int) reader["phone_number"];
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

    public IEnumerable<User> GetAllUsers()
    {
        var list = new List<User>();

        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "select * from user_order_database.user, user_order_database.credentials where credentials_username = username";
        var command = new MySqlCommand(commandString, connection);
        
        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new User
            {
                Id = (int) reader["uid"],
                FirstName = (string) reader["first_name"],
                LastName = (string) reader["last_name"],
                Email = (string) reader["email"],
                Number = (int) reader["phone_number"],
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
}