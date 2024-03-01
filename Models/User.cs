using Microsoft.AspNetCore.Identity;

namespace Gruppuppgift_BU2;

public class User : IdentityUser
{
    public List<Product> Cart = new List<Product>();
}



