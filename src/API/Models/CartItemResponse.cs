﻿namespace API.Models;
public class CartItemResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public double Price { get; set; }
    public double DiscountPercentage { get; set; }
    public IEnumerable<Link> Links { get; set; } = new List<Link>();
}
