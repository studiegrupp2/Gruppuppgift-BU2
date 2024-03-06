using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Gruppuppgift_BU2;

public class ProductService
{
    private ApplicationContext context;

    public ProductService(ApplicationContext context)
    {
        this.context = context;
    }

    public Product CreateProduct(
        string title,
        string description,
        string category,
        string size,
        string color,
        double price
    )
    {
        Product product = new Product(title, description, category, size, color, price);
        context.Products.Add(product);
        context.SaveChanges();
        return product;
    }

    public Product DeleteProduct(int productId)
    {
        Product? product = context.Products.Find(productId);

        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }
    
        context.Products.Remove(product);
        context.SaveChanges();

        return product;
    }

    public List<Product> GetAllProducts()
    {
        return context.Products.Include(product => product.Reviews).ToList();
    }

    public Review AddReview(string review, string userId, int productId)
    {
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
        context.Reviews.Add(_review);

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

    public Product AddProductToCart(int productId, string userId)
    {
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
    
        user.Cart.Add(product);
        context.SaveChanges();
        return product;
    }

    public Product RemoveProductFromCart(int productId, string userId)
    {
        User? user = context.Users.Find(userId);
        List<Product> Cart = user.Cart.ToList();

        Product? product = Cart.Find(item => item.Id == productId);

        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        user.Cart.Remove(product);
        context.SaveChanges();
        return product;
    }

    public List<Product> GetAllCartItems(string userId)
    {
        User? user = context.Users.Find(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
            // return new List<Product>();
        }
        //return user.Cart.Include(Cart)ToList();
        List<Product> Cart = user.Cart.ToList();
        return Cart;
        //return context.Products.Include(product => product.Reviews).ToList();

    }
}

