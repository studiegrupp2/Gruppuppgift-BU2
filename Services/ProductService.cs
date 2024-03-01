using System.Security.Claims;


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
       return context.Products.ToList();
    }
    
    // public Product AddRating(int id, double rating)
    // {
    //     Product? product = context.Products.Find(id);
    //     if(product == null)
    //     {
    //        throw new ArgumentException("Product not found.");
    //     }
    //     product.Rating++;
    //     context.SaveChanges();
    // return product;
    // }
    

    public Product AddReview(string review, int id, string name)
    {
        Product? product = context.Products.Find(id);
        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }
        Review _review = new Review(review, name);
        // _review.
        context.Reviews.Add(_review);
        // product.Reviews.Add(_review);
        // context.Products.Update(product);

        context.SaveChanges();
        
        return product;
    }
}