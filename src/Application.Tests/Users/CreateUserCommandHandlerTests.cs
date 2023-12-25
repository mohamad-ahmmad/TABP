using Application.Abstractions;
using Application.Users.Commands.Create;
using Application.Users.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using FluentAssertions;
using Moq;


namespace Application.Tests.Users;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IUsersRepository> usersRepositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IHasher> passwordHasherMock;

    private readonly CreateUserCommandHandler createUserCommandHandler;

    public CreateUserCommandHandlerTests()
    {
        unitOfWorkMock = new Mock<IUnitOfWork>();
        usersRepositoryMock = new Mock<IUsersRepository>();
        mapperMock = new Mock<IMapper>();
        passwordHasherMock = new Mock<IHasher>();

        createUserCommandHandler = new CreateUserCommandHandler(
            unitOfWorkMock.Object,
            usersRepositoryMock.Object,
            mapperMock.Object,
            passwordHasherMock.Object
        );
        var user = new User();
        mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserForCreateDTO>()))
            .Returns(user);
        mapperMock.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>()))
            .Returns(new UserDto { });
    }

    [Theory]
    [MemberData(nameof(UsernameEmailCases))]
    public async Task Handle_WithExistingUsernameEmail_ShouldReturnErrors(bool isUsernameExist, bool isEmailExist, IEnumerable<Error> errors, bool returned)
    {
        // Arrange
        var createUserCommand = new CreateUserCommand(new UserForCreateDTO()
        {
        });

        var cancellationToken = new CancellationToken();

        usersRepositoryMock.Setup(repo => repo.IsUsernameExistByAsync(It.IsAny<string>(), cancellationToken))
            .ReturnsAsync(isUsernameExist);
        usersRepositoryMock.Setup(repo => repo.IsEmailExistByAsync(It.IsAny<string>(), cancellationToken))
            .ReturnsAsync(isEmailExist);

        // Act
        var result = await createUserCommandHandler.Handle(createUserCommand, cancellationToken);

        // Assert
        result.Should().BeOfType<Result<UserDto?>>();
        result.IsSuccess.Should().Be(returned);
        result.Errors.Should().Equal(errors);
    }
    public static IEnumerable<object[]> UsernameEmailCases => new List<object[]>
        {
            new object[] { true, false, new List<Error> {UserErrors.UsernameAlreadyUsed}, false },
            new object[] { false, true, new List<Error> {UserErrors.EmailAlreadyUsed}, false },
            new object[] { true, true, new List<Error> {UserErrors.UsernameAlreadyUsed, UserErrors.EmailAlreadyUsed }, false },
            new object[] { false, false, new List<Error> {}, true },
        };
}

