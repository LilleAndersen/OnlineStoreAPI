using Microsoft.Win32.SafeHandles;
using MySqlConnector;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Services;

public class ProductService : IProductService
{
    public IEnumerable<Product> GetAllProducts()
    {
        var list = new List<Product>();

        using var connection = new MySqlConnection("server=192.168.1.10;uid=lilly;pwd=root;database=user_order_database");
        const string commandString = "select * from user_order_database.products";
        var command = new MySqlCommand(commandString, connection);
        
        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Product
            {
                Id = (int) reader["id"],
                Name = (string) reader["name"],
                Price = (float) reader["price"],
                Stock = (int) reader["stock"],
                ImageUrl = (string) reader["image"]
            });
        }

        return list;
    }
}