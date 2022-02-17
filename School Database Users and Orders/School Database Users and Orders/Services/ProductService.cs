using Microsoft.Win32.SafeHandles;
using MySqlConnector;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace School_Database_Users_and_Orders.Services;

// Does most of the grunt work, connects to the api and reads from it, thereof returns the request to the requester.
public class ProductService : IProductService
{

    // Gets a product based on a product id
    public Product GetProduct(int id)
    {
        var product = new Product();

        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "select * from user_order_database.products where product_id = @id"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);

        command.Parameters.AddWithValue("@id", id);

        connection.Open();

        using var reader = command.ExecuteReader();
        
        // Reads what the database returns after the SQL statement
        // Puts all relevant information in variables in JSON readable format
        while (reader.Read())
        {
            product.Id = (int) reader["product_id"];
            product.Name = (string) reader["name"];
            product.Price = (float) reader["price"];
            product.Stock = (int) reader["stock"];
            product.Description = (string) reader["description"];
            product.ImageUrl = (string) reader["image"];
        }

        return product;
    }
    
    // Gets all products
    public IEnumerable<Product> GetAllProducts()
    {
        var list = new List<Product>();

        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "select * from user_order_database.products"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);
        
        connection.Open();

        using var reader = command.ExecuteReader();
        
        // Reads what the database returns after the SQL statement
        // Puts all relevant information in variables in JSON readable format
        while (reader.Read())
        {
            list.Add(new Product
            {
                Id = (int) reader["product_id"],
                Name = (string) reader["name"],
                Price = (float) reader["price"],
                Stock = (int) reader["stock"],
                Description = (string) reader["description"],
                ImageUrl = (string) reader["image"]
            });
        }

        return list;
    }
}