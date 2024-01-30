using FluentValidation;

namespace Application.RoomTypes.Commands.CreateRoomType;
public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
{
    public CreateRoomTypeCommandValidator()
    {
        RuleFor(c => c.RoomType.Name)
            .NotEmpty();
            
    }
}

