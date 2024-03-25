using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

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
        double price,
        string thumbnail
    )
    {
        Product product = new Product(title, description, category, size, color, price, thumbnail);
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

    public Product UpdateProduct(
        int productId, 
        string title,
        string description,
        string category,
        string size,
        string color,
        double price,
        string thumbnail)
    {
        Product? product = context.Products.Find(productId);

        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }

        product.Title = title;
        product.Description = description;
        product.Category = category;
        product.Size = size;
        product.Color = color;
        product.Size = size;
        product.Price = price;
        product.Thumbnail = thumbnail;

        context.SaveChanges();

        return product;
    }

    public List<Product> GetAllProducts()
    {
        return context.Products.Include(product => product.Reviews).ToList();
    }

    public List<Order> GetAllOrders()
    {
        return context.Orders.Include(order => order.Items).Include(order => order.User).ToList();
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
        if (ratingValue < 0 || ratingValue > 5)
        {
            throw new ArgumentOutOfRangeException("Rating value out of range");
        }
        Rating userRating = new Rating(ratingValue, product);
        product.ratingList.Add(ratingValue);
        context.SaveChanges();

        return userRating;
    }

    public CartItem AddProductToCart(int productId, string userId)
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
        
        List<CartItem> cartList = context.CartItems.Include(cartItem => cartItem.Product).Where(u => u.User.Id == userId).ToList();
        if (cartList.Any(item => item.Product.Id == productId))
        {
            int index = cartList.FindIndex(item => item.Product.Id == productId);
            cartList[index].Quantity ++;

            context.SaveChanges();
            return cartList[index];
        }
         
        CartItem? cartItem = new CartItem(product, user, 1);
       
        context.CartItems.Add(cartItem);
        context.SaveChanges();
        return cartItem;
    }

    public CartItem? RemoveProductFromCart(int productId, string userId)
    {
        User? user = context.Users.Find(userId);

        List<CartItem> Cart = context.CartItems.Include(cartItem => cartItem.Product).Where(u => u.User.Id == userId).ToList();
        CartItem? product = Cart.Find(item => item.Product.Id == productId);

        if (product == null)
        {
            throw new ArgumentException("product not found");
        }
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        if (product.Quantity > 1)
        {
            product.Quantity --;

            context.SaveChanges();
            return product;
        }
        
        context.CartItems.Remove(product);
        context.SaveChanges();
        return product;
    }

    public List<CartItem> GetAllCartItems(string userId)
    {
        User? user = context.Users.Find(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        
        List<CartItem> cartItems = context.CartItems.Include(cartItem => cartItem.Product).Where(u => u.User.Id == userId).ToList();

        return cartItems;
    }

    public double GetCartItemsTotalPrice(string userId)
    {
        User? user = context.Users.Find(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }
        
        List<CartItem> cartItems = context.CartItems.Include(cartItem => cartItem.Product).Where(u => u.User.Id == userId).ToList();

        double sum = 0;
            foreach (CartItem item in cartItems)
            {
                double productPrice = item.TotalPrice(); 
                sum += productPrice;
            }

        return sum;
    }

    public List<CartItem> BuyItemsInCart(string userId)
    {
        User? user = context.Users.Find(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        List<CartItem> cartHistory = context.CartItems.Include(cartItem => cartItem.Product).Where(u => u.User.Id == userId).ToList();
        List<PurchasedItem> newOrder = new List<PurchasedItem>();
        foreach(CartItem item in cartHistory)
        {
            PurchasedItem purchased = new PurchasedItem(item);
            newOrder.Add(purchased);
            context.PurchasedItems.Add(purchased);
            context.CartItems.Remove(item);
        }
        
        Order order = new Order(newOrder, user);
        context.Orders.Add(order);
        user.Cart.Clear();
        context.SaveChanges();
        if (cartHistory == null || cartHistory.Count < 1)
        {
            return cartHistory = [];
        }
        
        return cartHistory;
    }
}

