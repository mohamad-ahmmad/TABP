﻿using Application.HotelTypes.Dtos;
using Domain.Entities;

namespace Application.Hotels.Dtos;
public class HotelDto
{
    public Guid Id { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public int NumberOfRooms { get; set; } = 0;
    public Guid? CityId { get; set; }
    public long StarRatingAcc { get; set; }
    public string Description { get; set; } = string.Empty;
    public long NumberOfPeopleRated { get; set; }
    public HotelTypeDto HotelType { get; set; } = null!;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string StreetNme { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public Guid? OwnerId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public bool IsAdmin { get; set; }
}
