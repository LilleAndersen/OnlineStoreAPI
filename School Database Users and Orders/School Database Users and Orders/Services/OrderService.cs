using MySqlConnector;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace School_Database_Users_and_Orders.Services;

public class OrderService : IOrderService
{
    public Order GetOrder(int id)
    {
        var Order = new Order();
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "select * from user_order_database.orders, user_order_database.user, user_order_database.credentials, user_order_database.addresses, user_order_database.post_place, user_order_database.products where orders.user_uid = user.uid and orders.product_id = products.product_id and orders.address_id = addresses.address_id and addresses.postal_number = post_place.postal_number and user.credentials_username = credentials.username and orders.id = @id";
        var command = new MySqlCommand(commandString, connection);

        command.Parameters.AddWithValue("@id", id);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            Order.Id = (int) reader[0];
            Order.User = new User
            {
                Id = (int) reader["uid"],
                FirstName = (string) reader["first_name"],
                LastName = (string) reader["last_name"],
                Credentials = new Credentials
                {
                    Username = (string) reader["username"],
                    Password = (string) reader["password"],
                    Token = (string) reader["token"],
                    AccessLevel = (int) reader["access_level"]
                },
                Email = (string) reader["email"],
                Number = (int) reader["phone_number"]
            };
            Order.Product = new Product
            {
                Id = (int) reader["product_id"],
                Name = (string) reader["name"],
                Description = (string) reader["description"],
                Price = (float) reader["price"],
                Stock = (int) reader["stock"]
            };
            Order.Address = new Address
            {
                AddressId = (int) reader["address_id"],
                AddressName = (string) reader["address_name"],
                AddressLine = (string) reader["address_line"],
                PostalNumber = new PostalNumber
                {
                    Number = (int) reader["postal_number"],
                    Place = (string) reader["postal_place"]
                },
                Country = (string) reader["country"]
            };
            Order.TotalPrice = (float) reader["total_price"];
            Order.OrderTimestamp = (DateTime) reader["order_timestamp"];
        }
        
        return Order;
    }
}