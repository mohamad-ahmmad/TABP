using Domain.Shared;

namespace Domain.Errors;
public class BookingErrors
{
    public readonly static Error ForbidToCheckoutCartItems = new("Checkout CartItems", "No permission");
    public readonly static Error ForbidToGetBookings = new("Get Bookings", "No permission");
}

