using Microsoft.AspNetCore.Identity;

namespace RestaurantOrderingSystem.Models
{
    //Allow Us to use the built in identity system with username,password,login controls
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
