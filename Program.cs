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
                "create_product",
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
        app.MapControllers();
        app.MapIdentityApi<User>();

        app.Run();
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
