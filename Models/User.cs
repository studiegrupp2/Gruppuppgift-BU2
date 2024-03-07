using Microsoft.AspNetCore.Identity;

namespace Gruppuppgift_BU2;

public class User : IdentityUser
{
    public List<Product> Cart { get; set; } = new List<Product>();

    public User() { }
}

// public class Cart {
//  [key]
//  public int CartID {get; set;}
//  public Product Product {get; set;}
//  public int Quantity {get; set;}
// }