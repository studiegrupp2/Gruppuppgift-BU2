
using Microsoft.EntityFrameworkCore;

namespace Gruppuppgift_BU2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql("Host=localhost;Database=Ecommerce;Username=postgres;Password=ecommerce"));
  
        builder.Services.AddAuthorization();

    
        builder.Services.AddEndpointsApiExplorer();
        
        // builder.Services.AddSwaggerGen();
        var app = builder.Build();

        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.Run();
    }
}
