using Application.Abstractions.Data;
using Application.Users.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Users.Commands.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IUsersRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork,
                                    IUsersRepository usersRepository,
                                    IMapper mapper)
    {
        _userRepo = usersRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepo.IsUsernameExistByAsync(request.UserForCreateDto.Username, cancellationToken))
        {
            return UserErrors.UsernameAlreadyUsed;
        }
        if(await _userRepo.IsEmailExistByAsync(request.UserForCreateDto.Email, cancellationToken))
        {
            return UserErrors.EmailAlreadyUsed;
        }
        if (request.UserForCreateDto.UserLevel == UserLevels.Admin)
        {
            return UserErrors.UnAuthorized;
        }
        var user = _mapper.Map<User>(request.UserForCreateDto);

        await _userRepo.AddUserAsync(user, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}



