using Domain.Shared;

namespace Domain.Errors;
public static class CartItemErrors
{
    public readonly static Error ForbidToCreateCartItem = new("Create a cart item", "No permission");
    
}
