using Microsoft.AspNetCore.Identity;

namespace Gruppuppgift_BU2;

public class User : IdentityUser
{
    public List<CartItem> Cart { get; set; } = new List<CartItem>();

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