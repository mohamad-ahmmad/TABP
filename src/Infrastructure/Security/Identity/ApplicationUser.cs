using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security.Identity;
public class ApplicationUser : IdentityUser
{
    public ICollection<CartItem> CartItems { get; set; } = [];
}
