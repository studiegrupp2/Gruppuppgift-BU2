namespace Gruppuppgift_BU2;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public double Price { get; set; }
    public string Thumbnail { get; set; }
    

    public List<double> ratingList { get; set; } = new List<double>();

    public double AverageRating()
    {
        return ratingList.Count > 0 ? ratingList.Average() : 0.0;
    }

    public List<Review> Reviews { get; set; } = new List<Review>();

    public Product() { }

    public Product(
        string title,
        string description,
        string category,
        string size,
        string color,
        double price,
        string thumbnail
    )
    {
        this.Title = title;
        this.Description = description;
        this.Category = category;
        this.Size = size;
        this.Color = color;
        this.Price = price;
        this.ratingList = ratingList;
        this.Reviews = Reviews;
        this.Thumbnail = thumbnail;
    }
}

public class Review
{
    public int Id { get; set; }
    public string UserReview { get; set; }
    public User User { get; set; }
    public Product? Product { get; set; }
    public string? ReviewUserName {get; set;}
    public Review() { }

    public Review(string userReview, User user, Product product)
    {
        this.UserReview = userReview;
        this.User = user;
        this.Product = product;
        this.ReviewUserName = user.UserName;
    }
}

public class Rating
{
    public int Id { get; set; }
    public double UserRating { get; set; }
    public Product Product { get; set; }

    public Rating() { }

    public Rating(double userRating, Product product)
    {
        this.UserRating = userRating;
        this.Product = product;
    }
}
