using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace Gruppuppgift_BU2;

public class ProductService
{
    private ApplicationContext context;

    public ProductService(ApplicationContext context)
    {
        this.context = context;
    }
    public Product CreateProduct(string title, string description, string category, string size, string color, double price)
    {
        Product product = new Product(title, description, category, size, color, price);
        context.Products.Add(product);
        context.SaveChanges();
        return product;
    }

    public List<Product> GetAllProducts()
    {
        return context.Products.Include(product => product.Reviews).ToList();
    }

    // public Product AddRating(int id, double rating)
    // {
    //     Product? product = context.Products.Find(id);
    //     double ratingList = context.
    //     if(product == null)
    //     {
    //        throw new ArgumentException("Product not found.");
    //     }
    //     product.AverageRating++;
    //     context.SaveChanges();
    // return product;
    // }


    public Review AddReview(string review, string userId, int productId)
    {
        // Product? product = context.Products.Find(product.id);
        Product? product = context.Products.Find(productId);
        User? user = context.Users.Find(userId);
        
        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        Review _review = new Review(review, user, product);
        // _review.
        context.Reviews.Add(_review);
        // product.Reviews.Add(_review);
        // context.Products.Update(product);

        context.SaveChanges();

        return _review;
    }

    public Rating AddRating(int productId, double ratingValue)
    {
        Product? product = context.Products.Find(productId);
        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }
        Rating userRating = new Rating(ratingValue, product);
        product.ratingList.Add(ratingValue);
        context.SaveChanges();

        return userRating;
    }

    
}