using Domain.Entities;

namespace Application.Rooms.Dtos;
public class RoomDto
{
    public Guid Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public Guid RoomInfoId { get; set; }
    public int PricePerDay { get; set; }
    public double DiscountPercentage { get; set; }
    public DateTime Created { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime LastModified { get; set; }
    public Guid LastModifiedBy { get; set; }
    public bool IsAdmin { get; set; }
}
