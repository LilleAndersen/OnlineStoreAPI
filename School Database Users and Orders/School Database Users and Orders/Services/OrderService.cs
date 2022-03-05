using MySqlConnector;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Models;
using School_Database_Users_and_Orders.Models.Requests;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace School_Database_Users_and_Orders.Services;

// Does most of the grunt work, connects to the api and reads from it, thereof returns the request to the requester.
public class OrderService : IOrderService
{
    // Gets and order based on id
    public Order GetOrder(int id)
    {
        var Order = new Order();
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "select * from user_order_database.orders, user_order_database.user, user_order_database.credentials, user_order_database.addresses, user_order_database.post_place where orders.user_uid = user.uid and orders.address_id = addresses.address_id and addresses.postal_number = post_place.postal_number and user.credentials_username = credentials.username and orders.id = @id"; // The SQL statement itself, what gets executed in the database
        const string commandStringProducts = "select products.*, orders_has_products.quantity, user_order_database.orders.id from user_order_database.products, user_order_database.orders, user_order_database.orders_has_products where id = orders_id and products_id = product_id and user_uid = @id"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);
        var productCommand = new MySqlCommand(commandStringProducts, connection);

        // Variables for the SQL statement, you put @user in the SQL statement and define it outside the statement
        command.Parameters.AddWithValue("@id", id);
        productCommand.Parameters.AddWithValue("@id", id);

        connection.Open();

        using var reader = command.ExecuteReader();
        
        // Reads what the database returns after the SQL statement
        // Puts all relevant information in variables in JSON readable format
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
            Order.Status = (string) reader["status"];
            Order.OrderTimestamp = (DateTime) reader["order_timestamp"];
        }
        
        var list = new List<OrderProduct>();
        
        reader.Close();
        
        using var productReader = productCommand.ExecuteReader();

        // Reads what the database returns after the SQL statement
        // Puts all relevant information in variables in JSON readable format
        while (productReader.Read())
        {
            list.Add(new OrderProduct
            {
                OrderId = Order.Id,
                Product = new Product
                {
                    Id = (int) productReader["product_id"],
                    Name = (string) productReader["name"],
                    Price = (float) productReader["price"],
                    Stock = (int) productReader["stock"],
                    Description = (string) productReader["description"],
                    ImageUrl = (string) productReader["image"]
                },
                Quantity = (int) productReader["quantity"]
            });
        }

        Order.Products = list;
        
        return Order;
    }

    // Gets all orders based on the user id
    public IEnumerable<Order> GetUserOrders(int id)
    {
        var products = new List<OrderProduct>();
        var orders = new List<Order>();
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        
        const string productsCommandString = "select products.*, orders_has_products.quantity, user_order_database.orders.id from user_order_database.products, user_order_database.orders, user_order_database.orders_has_products  where id = orders_id  and products_id = product_id  and user_uid = @id"; // The SQL statement itself, what gets executed in the database
        var productsCommand = new MySqlCommand(productsCommandString, connection);
        
        const string ordersCommandString = "select *from user_order_database.orders, user_order_database.user, user_order_database.credentials, user_order_database.addresses, user_order_database.post_place where orders.user_uid = user.uid and orders.address_id = addresses.address_id and addresses.postal_number = post_place.postal_number and user.credentials_username = credentials.username and user_uid = @id"; // The SQL statement itself, what gets executed in the database
        var ordersCommand = new MySqlCommand(ordersCommandString, connection);

        // Variables for the SQL statement, you put @user in the SQL statement and define it outside the statement
        ordersCommand.Parameters.AddWithValue("@id", id);
        productsCommand.Parameters.AddWithValue("@id", id);
        
        
        
        // List of all products for user
        // Sorted based on which order
        
        connection.Open();

        using var productsReader = productsCommand.ExecuteReader();
        
        // Reads what the database returns after the SQL statement
        // Puts all relevant information in variables in JSON readable format
        while (productsReader.Read())
        {
            products.Add(new OrderProduct
            {
                OrderId = (int) productsReader ["id"],
                Product = new Product
                {
                    Id = (int) productsReader["product_id"],
                    Name = (string) productsReader["name"],
                    Price = (float) productsReader["price"],
                    Stock = (int) productsReader["stock"],
                    Description = (string) productsReader["description"],
                    ImageUrl = (string) productsReader["image"]
                },
                Quantity = (int) productsReader["quantity"]
            });
        }
        
        // Get all orders of a user
        // While getting orders checking the list
        // Find related ones in list and add to current order
        
        productsReader.Close();
        
        using var ordersReader = ordersCommand.ExecuteReader();
        
        // Reads what the database returns after the SQL statement
        // Puts all relevant information in variables in JSON readable format
        while (ordersReader.Read())
        {
            var orderId = (int) ordersReader[0];
            orders.Add(new Order
            {
                Id = orderId,
                User = new User
                {
                Id = (int) ordersReader["uid"],
                FirstName = (string) ordersReader["first_name"],
                LastName = (string) ordersReader["last_name"],
                Credentials = new Credentials
                {
                    Username = (string) ordersReader["username"],
                    Password = (string) ordersReader["password"],
                    Token = (string) ordersReader["token"],
                    AccessLevel = (int) ordersReader["access_level"]
                },
                Email = (string) ordersReader["email"],
                Number = (int) ordersReader["phone_number"]
                },
                Products = products.Where(p => p.OrderId == orderId),
                Address = new Address
                {
                    AddressId = (int) ordersReader["address_id"],
                    AddressName = (string) ordersReader["address_name"],
                    AddressLine = (string) ordersReader["address_line"],
                    PostalNumber = new PostalNumber
                    {
                        Number = (int) ordersReader["postal_number"],
                        Place = (string) ordersReader["postal_place"]
                    },
                Country = (string) ordersReader["country"]
                },
                TotalPrice = (float) ordersReader["total_price"],
                Status = (string) ordersReader["status"],
                OrderTimestamp = (DateTime) ordersReader["order_timestamp"],
            });
        }

        return orders;
    }

    // Creates an order based on the user id, address id and totalprice
    public bool CreateOrder(int id, int addressId, float totalPrice)
    {
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "insert into user_order_database.orders (user_uid, address_id, total_price) values (@id, @addressId, @totalPrice)"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);

        // Variables for the SQL statement, you put @user in the SQL statement and define it outside the statement
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@addressId", addressId);
        command.Parameters.AddWithValue("@totalPrice", totalPrice);


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

    // Adds a product to the order based on the order id, product id and quantity
    public bool AddProductToOrder(int orderId, int productId, int quantity)
    {
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "insert into user_order_database.orders_has_products (orders_id, products_id, quantity) values (@orderId, @productId, @quantity)"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);

        // Variables for the SQL statement, you put @user in the SQL statement and define it outside the statement
        command.Parameters.AddWithValue("@orderId", orderId);
        command.Parameters.AddWithValue("@productId", productId);
        command.Parameters.AddWithValue("@quantity", quantity);


        // Reads what the database returns after the SQL statement
        // Puts all relevant information in variables in JSON readable format
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

    // Updates the order status based on the new status and order id
    public bool UpdateOrderStatus(string status, int id)
    {
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "update user_order_database.orders set status = @status where id = @id"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);
        
        // Variables for the SQL statement, you put @user in the SQL statement and define it outside the statement
        command.Parameters.AddWithValue("@status", status);
        command.Parameters.AddWithValue("@id", id);

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

    public int CreateAddress(string addressName, string addressLine, string postalNumber, string country)
    {
        var Address = new int();
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        const string commandString = "insert into user_order_database.addresses (address_name, address_line, country, postal_number) values (@addressName, @addressLine, @country, @postalNumber)"; // The SQL statement itself, what gets executed in the database
        const string addressIdCommandString = "select user_order_database.addresses.address_id from user_order_database.addresses where addresses.address_line = @addressLine and addresses.address_name = @addressName and addresses.country = @country and addresses.postal_number = @postalNumber"; // The SQL statement itself, what gets executed in the database
        var command = new MySqlCommand(commandString, connection);
        var addressIdcommand = new MySqlCommand(addressIdCommandString, connection);

        // Variables for the SQL statement, you put @user in the SQL statement and define it outside the statement
        command.Parameters.AddWithValue("@addressLine", addressLine);
        command.Parameters.AddWithValue("@addressName", addressName);
        command.Parameters.AddWithValue("@postalNumber", postalNumber);
        command.Parameters.AddWithValue("@country", country);
        
        addressIdcommand.Parameters.AddWithValue("@addressLine", addressLine);
        addressIdcommand.Parameters.AddWithValue("@addressName", addressName);
        addressIdcommand.Parameters.AddWithValue("@postalNumber", postalNumber);
        addressIdcommand.Parameters.AddWithValue("@country", country);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            using var reader = addressIdcommand.ExecuteReader();
            
            while (reader.Read())
            {
                Address = (int) reader[0];
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Address;
    }
}