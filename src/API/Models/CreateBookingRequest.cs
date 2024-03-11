using Microsoft.AspNetCore.Mvc;

namespace API.Models;
public class CreateBookingRequest
{
    
    public string CardDetailsToken { get; set; }
    public string IdempotencyKey { get; set; }
}

