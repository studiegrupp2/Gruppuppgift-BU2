namespace Gruppuppgift_BU2;

using System;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public double Price { get; set; }
    public double AverageRating { get; set; }
    public List<ReviewDto> Reviews { get; set; }

    public ProductDto(Product product)
    {
        this.Title = product.Title;
        this.Description = product.Description;
        this.Category = product.Category;
        this.Size = product.Size;
        this.Color = product.Color;
        this.Price = product.Price;
        this.AverageRating = product.AverageRating();
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

    public CustomerController(
        ProductService productService,
        RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager,
        ApplicationContext context
    )
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
        List<ProductDto> productDtos = productService
            .GetAllProducts()
            .Select(product => new ProductDto(product))
            .ToList();
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
    }

    [HttpPost("product/{id}/rating")]
    [Authorize]
    public IActionResult AddRating([FromQuery] double userRating, int id)
    {
        if (userRating < 0 || userRating > 5)
        {
            return BadRequest("Not valid rating value.");
        }
        int productId = id;
        Rating rating = productService.AddRating(productId, userRating);
        return Ok("Rating submitted succefully.");
    }

    [HttpPost("product/{id}/cart")]
    [Authorize]
    public IActionResult AddProductToCart(int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Product cart = productService.AddProductToCart(id, userId);
        return Ok(cart);
    }

    [HttpDelete("product/{id}/cart")]
    [Authorize]
    public IActionResult RemoveFromCart(int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Product product = productService.RemoveProductFromCart(id, userId);
        return Ok(product);
    }

    [HttpGet("cart")]
    [Authorize]
    public IActionResult GetCartItems()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(productService.GetAllCartItems(userId));
    }
}
