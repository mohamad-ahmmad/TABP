﻿
namespace API.Models;
public class DiscountResponse
{
    public Guid Id { get; set; }
    public double DiscountPercentage { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Guid RoomId { get; set; }

}
