using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gruppuppgift_BU2;

public class ApplicationContext : IdentityDbContext<User>
{
    public DbSet<Product> Products { get; set;}
    public DbSet<Review> Reviews {get; set; }
    public DbSet<CartItem> CartItems {get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}
}