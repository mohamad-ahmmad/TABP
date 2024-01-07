using Domain.Common;

namespace Domain.Entities;
public class Owner : BaseSoftDeletableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}

