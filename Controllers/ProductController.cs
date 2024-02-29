using Microsoft.AspNetCore.Mvc;

namespace Gruppuppgift_BU2;

public class CreateProductDto 
{
    public string Title {get; set;}
    public string Description { get; set;}
    public string Category {get; set;}
    public string Size {get; set;}
    public string Color {get; set;}
    public double Price {get; set;}
    public CreateProductDto(string title, string description, string category, string size, string color, double price)
    {
        this.Title = title;
        this.Description = description;
        this.Category = category;
        this.Size = size;
        this.Color = color;
        this.Price = price;
    }
}

[ApiController]
[Route("")]
public class ProductController : ControllerBase
{
    private ProductService productService;

    public ProductController(ProductService productService)
    {
        this.productService = productService;
    }
    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto dto)
    {
        try 
        {
            productService.CreateProduct(dto.Title, dto.Description, dto.Category, dto.Size, dto.Color, dto.Price);
            return Ok("Product" + dto.Title + "added");
        }
        catch (ArgumentException)
        {
            return BadRequest();
        } 
    }
}