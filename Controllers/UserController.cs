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
    public int Id { get; set; }
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
        this.Id = product.Id;
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
        this.inputName = review.ReviewUserName;
    }

    public ReviewDto() { }
}

public class CartItemDto
{
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }

    public CartItemDto(CartItem cartItem)
    {
        this.Product = cartItem.Product;
        this.Quantity = cartItem.Quantity;
        this.TotalPrice = cartItem.TotalPrice();
    }

    public CartItemDto() { }
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

        try
        {
            Review _review = productService.AddReview(dto.inputReview, userId, productId);
            ReviewDto output = new ReviewDto(_review);
            return Ok(output);
        }
        catch (ArgumentException)
        {
            return BadRequest("User or product not found");
        }
    }

    [HttpPost("product/{id}/rating")]
    [Authorize]
    public IActionResult AddRating([FromQuery] double userRating, int id)
    {
        int productId = id;
        try
        {
            Rating rating = productService.AddRating(productId, userRating);
            return Ok("Rating submitted succefully.");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Value out of range");
        }
        catch (ArgumentException)
        {
            return BadRequest("Product not found");
        }
    }

    [HttpPost("product/{id}/cart")]
    [Authorize]
    public IActionResult AddProductToCart(int id)
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CartItem cart = productService.AddProductToCart(id, userId);

            CartItemDto output = new CartItemDto(cart);
            return Ok(output);
        }
        catch (ArgumentException)
        {
            return BadRequest("Product not found");
        }
    }
    [HttpDelete("product/{id}/cart")]
    [Authorize]
    public IActionResult RemoveFromCart(int id)
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CartItem cart = productService.RemoveProductFromCart(id, userId);
            CartItemDto output = new CartItemDto(cart);
            return Ok(output);
        }
        catch (ArgumentException)
        {
            return BadRequest("Product or user not found");
        }
    }

    [HttpGet("cart")]
    [Authorize]
    public IActionResult GetCartItems()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        double Total = productService.GetCartItemsTotalPrice(userId);
        return Ok(
            new
            {
                cart = productService
                    .GetAllCartItems(userId)
                    .Select(item => new CartItemDto(item))
                    .ToList(),
                Total
            }
        );
    }

    [HttpPut("product/cart/buy")]
    [Authorize]
    public IActionResult BuyItems()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            double Total = productService.GetCartItemsTotalPrice(userId);
            return Ok(
                new
                {
                    Order = productService
                        .BuyItemsInCart(userId)
                        .Select(item => new CartItemDto(item))
                        .ToList(),
                    Total
                }
            );
        }
        catch (ArgumentException)
        {
            return BadRequest("No items in cart");
        }
    }
}
