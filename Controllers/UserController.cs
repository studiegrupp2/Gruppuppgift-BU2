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

public class ReviewDto
{
    public string Review { get; set; }

    public ReviewDto(string review)
    {
        this.Review = review;
    }
}

[ApiController]
[Route("store")]
public class CustomerController : ControllerBase
{
    private ProductService productService;
    RoleManager<IdentityRole> roleManager;
    UserManager<User> userManager;

    public CustomerController(ProductService productService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager )
    {
        this.productService = productService;
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    [HttpGet("products")]
    [Authorize]
    public IActionResult GetAllProducts()
    {
        return Ok(productService.GetAllProducts());
    }

    [HttpPost("product/{id}")]
    [Authorize]
    public IActionResult PostReview([FromBody] ReviewDto dto, int id)
    {
        string? name = User.FindFirstValue(ClaimTypes.Name);
        string review = dto.Review;
        return Ok(productService.AddReview(review, id, name));
    }
  
}
