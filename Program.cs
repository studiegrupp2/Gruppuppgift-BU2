using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gruppuppgift_BU2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://localhost:5000").AllowAnyHeader().AllowAnyMethod();
                }
            );
        });
        
        builder.Services.AddDbContext<ApplicationContext>(options =>
            options.UseNpgsql(
                "Host=localhost;Database=Ecommerce;Username=postgres;Password=password"
            )
        );

        builder.Services.AddControllers();
        builder.Services.AddTransient<IClaimsTransformation, UserClaimsTransformation>();
        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        builder
            .Services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddApiEndpoints();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "manager",
                policy =>
                {
                    policy.RequireAuthenticatedUser().RequireRole("manager");
                }
            );
        });

        builder.Services.AddScoped<ProductService>();

        builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();
        var app = builder.Build();

        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors(MyAllowSpecificOrigins);
        app.MapControllers();
        app.MapIdentityApi<User>();

        Task.Run(async () =>
        {
            await CreateRole(app.Services);
            await CreateUser(app.Services);
        });
        app.Run();
    }

    static async Task CreateRole(IServiceProvider provider)
    {
        var scope = provider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        IdentityRole? exisiting = await roleManager.FindByNameAsync("manager");
        if (exisiting == null)
        {
            await roleManager.CreateAsync(new IdentityRole("manager"));
        }
    }

    static async Task CreateUser(IServiceProvider provider)
    {
        var scope = provider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        User? existingUser = await userManager.FindByNameAsync("chef@hotmail.com");
        var hasher = new PasswordHasher<User>();
        if (existingUser == null)
        {
            var adminUser = new User()
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                UserName = "chef@hotmail.com",
                Email = "chef@hotmail.com",
                NormalizedUserName = "chef@hotmail.com".ToUpper(),
                NormalizedEmail = "chef@hotmail.com".ToUpper(),
                PasswordHash = hasher.HashPassword(null, "Admin123!"),
                EmailConfirmed = true,
                LockoutEnabled = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            await userManager.CreateAsync(adminUser);
            await userManager.ChangePasswordAsync(adminUser, "Admin123!", "Chef123!");

            await userManager.AddToRoleAsync(adminUser, "manager");
        }
    }

    public class UserClaimsTransformation : IClaimsTransformation
    {
        UserManager<User> userManager;

        public UserClaimsTransformation(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity claims = new ClaimsIdentity();

            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != null)
            {
                var user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    foreach (var userRole in userRoles)
                    {
                        claims.AddClaim(new Claim(ClaimTypes.Role, userRole));
                    }
                }
            }
            principal.AddIdentity(claims);
            return await Task.FromResult(principal);
        }
    }
}
