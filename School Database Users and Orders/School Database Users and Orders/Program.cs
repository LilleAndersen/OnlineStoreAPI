using System.Net;
using System.Security.Cryptography.X509Certificates;
using School_Database_Users_and_Orders.Interfaces;
using School_Database_Users_and_Orders.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Links the Interface and Service with each other
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Makes sure a certificate is being used to make sure the api uses HTTPS and therefore is encrypted
/*builder.WebHost.UseKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 5000, listenOptions =>
    {
        listenOptions.UseHttps(new X509Certificate2("cert.pfx", "root"));
    });
}); */

// Describes the CorsPolicy which basically allows any connection from anywhere, the api has a security measure which
// blocks a bunch of requests from different sources and therefore this is needed to use the api
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
{
    b.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();


app.UseHsts();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();