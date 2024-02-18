using Domain.Entities;

namespace Infrastructure.Persistence.Repositories.Hotels.Model;
internal class HotelAndDiscount
{
    public Hotel Hotel { get; set; } = new Hotel();
    public Discount? Discount { get; set; } = null!;
}
