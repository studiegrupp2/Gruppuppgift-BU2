using Microsoft.AspNetCore.Identity;

namespace Gruppuppgift_BU2;

public class User : IdentityUser
{
    public List<CartItem> Cart { get; set; } = new List<CartItem>();
    public List<PurchasedItem> PurchaseHistory { get; set; } = new List<PurchasedItem>();
    public List<Order> OrderHistory { get; set; } = new List<Order>();
    public User() { }
}

public class CartItem
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public User User { get; set; }
    public double TotalPrice()
    {
        return this.Quantity * this.Product.Price;
    }
    public CartItem() { }
    public CartItem(Product product, User user, int quantity)
    {
        this.Product = product;
        this.User = user;
        this.Quantity = quantity;
    }
}

public class PurchasedItem
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public User User { get; set; }
    public string ProductTitle { get; set; }
    public double ProductPrice { get; set;}

    public double TotalPrice()
    {
        return this.Quantity * this.ProductPrice;
    }
    

    public PurchasedItem() { }
    public PurchasedItem(CartItem cartItem)
    {
        this.Product = cartItem.Product;
        this.User = cartItem.User;
        this.Quantity = cartItem.Quantity;
        this.ProductTitle = cartItem.Product.Title;
        this.ProductPrice = cartItem.Product.Price;

    }
}

public class Order
{
    public int Id {get; set;}
    public List<PurchasedItem> Items {get; set;}
    public User User { get; set; }
    public System.DateTime OrderDate {get; set;}
    public double TotalOrderPrice()
    {
        double sum = 0;
        foreach(PurchasedItem item in Items) {
            sum += item.TotalPrice();
        }

        return sum;
    }

    public Order(List<PurchasedItem> items, User user){
        this.OrderDate = DateTime.Now;
        this.Items = items;
        this.User = user;
    }

    public Order() { }
}