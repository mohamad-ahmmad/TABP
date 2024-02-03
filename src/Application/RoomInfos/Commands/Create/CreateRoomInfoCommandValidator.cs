using FluentValidation;

namespace Application.RoomInfos.Commands.Create;
public class CreateRoomInfoCommandValidator : AbstractValidator<CreateRoomInfoCommand>  
{
    public CreateRoomInfoCommandValidator()
    {
        //Add validation
        RuleFor(c => c.RoomInfoForCreateDto.AdultsCapacity)
            .NotEmpty();
        RuleFor(c=> c.RoomInfoForCreateDto.Description) 
            .NotEmpty();
    }
}
