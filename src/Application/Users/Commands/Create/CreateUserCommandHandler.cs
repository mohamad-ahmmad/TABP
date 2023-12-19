using Application.Abstractions;
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
    private readonly IHasher _passwordHasher;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork,
                                    IUsersRepository usersRepository,
                                    IMapper mapper,
                                    IHasher passwordHasher)
    {
        _userRepo = usersRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }
    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        if (await _userRepo.IsUsernameExistByAsync(request.UserForCreateDto.Username, cancellationToken))
        {
            errors.Add(UserErrors.UsernameAlreadyUsed);
        }
        if(await _userRepo.IsEmailExistByAsync(request.UserForCreateDto.Email, cancellationToken))
        {
            errors.Add(UserErrors.EmailAlreadyUsed);
        }
        if(errors.Any())
        {
            return errors;
        }

        var user = _mapper.Map<User>(request.UserForCreateDto);

        user.Password = _passwordHasher.Hash(user.Password);

        await _userRepo.AddUserAsync(user, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}



