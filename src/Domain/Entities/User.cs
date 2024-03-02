using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class User : BaseSoftDeletableEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public UserLevels UserLevel { get; set; } = UserLevels.User;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
