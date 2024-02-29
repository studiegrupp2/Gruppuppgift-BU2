namespace Gruppuppgift_BU2;

public class Product
{
    public int Id {get; set;}
    public string Title {get; set;}
    public string Description { get; set;}
    public string Category {get; set;}
    public string Size {get; set;}
    public string Color {get; set;}
    public double Price {get; set;}
    public double Rating {get; set;}
    public List<string> Reviews {get; set;} = new List<string>(); 


    public Product() { }
    public Product(string title, string description, string category, string size, string color, double price)
    {
        this.Id = Id;
        this.Title = title;
        this.Description = description;
        this.Category = category;
        this.Size = size;
        this.Color = color;
        this.Price = price;
        this.Rating = 0;
        this.Reviews = Reviews;
    }
}