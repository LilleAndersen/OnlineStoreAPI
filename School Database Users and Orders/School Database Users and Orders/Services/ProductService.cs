using Microsoft.Win32.SafeHandles;
using MySqlConnector;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;

namespace School_Database_Users_and_Orders.Services;

public class ProductService : IProductService
{

    public Product GetProduct(int id)
    {
        var product = new Product();

        using var connection = new MySqlConnection("server=10.100.0.125;uid=soni;pwd=Passord01;database=online_store");
        const string commandString = "select * from user_order_database.products where id = @id";
        var command = new MySqlCommand(commandString, connection);

        command.Parameters.AddWithValue("@id", id);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            product.Id = (int) reader["id"];
            product.Name = (string) reader["name"];
            product.Price = (float) reader["price"];
            product.Stock = (int) reader["stock"];
            product.ImageUrl = (string) reader["image_url"];
        }

        return product;
    }
    
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