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
    public List<Review> Reviews {get; set;} = new List<Review>(); 
    

    public Product() { }
    public Product(string title, string description, string category, string size, string color, double price)
    {
        this.Title = title;
        this.Description = description;
        this.Category = category;
        this.Size = size;
        this.Color = color;
        this.Price = price;
        this.Rating = 0.0;
        this.Reviews = Reviews;
    }
}

public class Review
{
    public int Id {get; set;}
    public string UserReview {get; set;}
    public User User {get; set;}
    // public int ProductId { get; set; }
    public Product Product {get; set;}

    public Review() { }
    public Review(string userReview, User user, Product product) {
        this.UserReview = userReview;
        this.User = user;
        this.Product = product;
        //this.User = user;
    }
}

/*
public string returnReview()
{
    return userReview + User.username;
}
*/
