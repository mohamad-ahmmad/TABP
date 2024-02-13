using FluentValidation;

namespace Application.Rooms.Commands.Create;
public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(e => e.RoomForCreationDto.RoomNumber).NotEmpty();
        RuleFor(e => e.RoomForCreationDto.PricePerDay).NotEmpty();
        RuleFor(e => e.RoomForCreationDto.RoomInfoId).NotEmpty();
    }
}
