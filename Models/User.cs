using Microsoft.AspNetCore.Identity;

namespace Gruppuppgift_BU2;

public class User : IdentityUser
{
    public List<CartItem> Cart { get; set; } = new List<CartItem>();
   // public List<PurchaseHistoryItem> History {get; set; }
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

    public PurchasedItem() { }
    public PurchasedItem(CartItem cartItem)
    {
        this.Product = cartItem.Product;
        this.User = cartItem.User;
        this.Quantity = cartItem.Quantity;
    }
}

public class Order
{
    public int Id {get; set;}
    public List<PurchasedItem> Items {get; set;}
    public User User { get; set; }
    public System.DateTime OrderDate {get; set;}

    public Order(List<PurchasedItem> items, User user){
        this.OrderDate = DateTime.Now;
        this.Items = items;
        this.User = user;
    }

    public Order() { }
}