using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gruppuppgift_BU2;

public class ApplicationContext : IdentityDbContext 
{
    public DbSet<Product> Products { get; set;}
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}
}