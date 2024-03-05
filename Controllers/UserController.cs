namespace Gruppuppgift_BU2;

using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Identity;


// public class CreateUser{}

public class RatingDto
{
    double Rating { get; set; }

    public RatingDto(int rating)
    {
        this.Rating = rating;
    }
}

public class ProductDto
{
    public string Title {get; set;}
    public string Description { get; set;}
    public string Category {get; set;}
    public string Size {get; set;}
    public string Color {get; set;}
    public double Price {get; set;}
    public double Rating {get; set;}
    public List<ReviewDto> Reviews { get; set; }

    public ProductDto(Product product)
    {
        this.Title = product.Title;
        this.Description = product.Description;
        this.Category = product.Category;
        this.Size = product.Size;
        this.Color = product.Color;
        this.Price = product.Price;
        this.Rating = product.Rating;
        this.Reviews = product.Reviews.Select(review => new ReviewDto(review)).ToList();
    }
}

public class ReviewDto
{
    public string inputReview { get; set; }
    public string? inputName { get; set; }

    public ReviewDto(Review review)
    {
        this.inputReview = review.UserReview;
        this.inputName = review.User.UserName; 
    }
    public ReviewDto() { }
}

[ApiController]
[Route("store")]
public class CustomerController : ControllerBase
{
    private ProductService productService;
    RoleManager<IdentityRole> roleManager;
    UserManager<User> userManager;
    private ApplicationContext context;

    public CustomerController(ProductService productService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationContext context)
    {
        this.productService = productService;
        this.roleManager = roleManager;
        this.userManager = userManager;
        this.context = context;
    }

    [HttpGet("products")]
    [Authorize]
    public IActionResult GetAllProducts()
    {
        List<ProductDto> productDtos = productService.GetAllProducts().Select(product => new ProductDto(product)).ToList();
        return Ok(productDtos);
    }

    [HttpPost("product/{id}")]
    [Authorize]
    public IActionResult PostReview([FromBody] ReviewDto dto, int id)
    {
        int productId = id;
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Review _review = productService.AddReview(dto.inputReview, userId, productId);

        ReviewDto output = new ReviewDto(_review);
        return Ok(output);

        // ---
        //string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //User? user = context.Users.Find(userId);
       // string reviewInput = dto.inputReview;

        //Review _review = productService.AddReview(reviewInput, user, product);

        // ReviewDto output = new ReviewDto(_review);
        // return Ok(output);
    }

}
