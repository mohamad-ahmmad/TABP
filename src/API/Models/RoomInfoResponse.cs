using Application.Rooms.Dtos;
using Application.RoomTypes.Dtos;
using Domain.Entities;

namespace API.Models;
public class RoomInfoResponse
{
    public Guid Id { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string Description { get; set; } = string.Empty;
    public RoomTypeDto? RoomType { get; set; } = null!;
    public List<RoomDto> Rooms { get; set; } = new();
    public Guid? HotelId { get; set; }
    public List<Link> Links { get; set; } = new List<Link>();
}

