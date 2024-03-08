using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Gruppuppgift_BU2;

//Kod för att DateTime ska funka
public static class MyModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
public class ApplicationContext : IdentityDbContext<User>
{
    public DbSet<Product> Products { get; set;}
    public DbSet<Review> Reviews {get; set; }
    public DbSet<CartItem> CartItems {get; set; }
    public DbSet<PurchasedItem> PurchasedItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}
}