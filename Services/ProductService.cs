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
}