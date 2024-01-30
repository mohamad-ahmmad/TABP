using Application.Abstractions.Messaging;
using Application.RoomTypes.Dtos;
using AutoMapper;
using Domain.Shared;

namespace Application.RoomTypes.Commands.CreateRoomType;
public class CreateRoomTypeCommandHandler : ICommandHandler<CreateRoomTypeCommand, RoomTypeDto>
{
    public CreateRoomTypeCommandHandler(IMapper mapper)
    {
        
    }
    public Task<Result<RoomTypeDto>> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
    {

        throw new NotImplementedException();
    }
}

