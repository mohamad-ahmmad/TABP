using FluentValidation;

namespace Application.CartItems.Commands.AddCartItem;
public class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>  
{
    public AddCartItemCommandValidator()
    {
        
    }
}
