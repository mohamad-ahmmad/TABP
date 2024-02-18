using Domain.Common;

namespace Domain.Entities;
public class Discount : BaseSoftDeletableEntity
{
    public double DiscountPercentage { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate {get; set;}
    public Guid? RoomId { get; set;}
    public Room? Room { get; set;} = null!;
}

