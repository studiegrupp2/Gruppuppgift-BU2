using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gruppuppgift_BU2;

public class CreateProductDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public double Price { get; set; }

    public CreateProductDto(
        string title,
        string description,
        string category,
        string size,
        string color,
        double price
    )
    {
        this.Title = title;
        this.Description = description;
        this.Category = category;
        this.Size = size;
        this.Color = color;
        this.Price = price;
    }
}

public class OrderDto
{
    public List<PurchasedItem> Items {get; set;}
    //public string? UserEmail {get; set;}
    public System.DateTime OrderDate {get; set;}

    public OrderDto(Order order)
    {
        this.Items = order.Items;
       // this.UserEmail = order.User.Email;
        this.OrderDate = order.OrderDate;
    }

    public OrderDto() {}
}

[ApiController]
[Route("manager")]
public class ProductController : ControllerBase
{
    private ProductService productService;

    public ProductController(ProductService productService)
    {
        this.productService = productService;
    }

    [HttpPost("create")]
    [Authorize("manager")]
    public IActionResult CreateProduct([FromBody] CreateProductDto dto)
    {
        try
        {
            productService.CreateProduct(
                dto.Title,
                dto.Description,
                dto.Category,
                dto.Size,
                dto.Color,
                dto.Price
            );
            return Ok("Product " + dto.Title + " added");
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
    }

    [HttpDelete("delete/{id}")]
    [Authorize("manager")]
    public IActionResult DeleteProduct(int Id)
    {
        int productId = Id;
        Product product = productService.DeleteProduct(productId);

        if (product == null)
        {
            return NotFound();
        }

        return Ok("Product " + productId + " is deleted");
    }

    [HttpPut("update/{id}")]
    [Authorize("manager")]
    public IActionResult UpdateProduct([FromBody] CreateProductDto dto, int id)
    {
        int productId = id;
        try
        {
            productService.UpdateProduct(
                productId,
                dto.Title,
                dto.Description,
                dto.Category,
                dto.Size,
                dto.Color,
                dto.Price
            );
            return Ok("Product " + dto.Title + " updated");
        }
        catch (ArgumentException)
        {
            return BadRequest("Product not found.");
        }
    }

//funkar ej
    // [HttpGet("orders")]
    // [Authorize("manager")]
    // public IActionResult GetAllOrders()
    // {
    //     List<OrderDto> orderDtos = productService.GetAllOrders().ToList().Select(order => new OrderDto(order)).ToList();
    //     return Ok(orderDtos);
    // }
}
