using Domain.Shared;

namespace Domain.Errors;
public static class CartItemErrors
{
    public readonly static Error ForbidToCreateCartItem = new("Create a cart item", "No permission");
    public readonly static Error ForbidToGetCartItems = new("Forbid cart item", "No permission");
    
}
