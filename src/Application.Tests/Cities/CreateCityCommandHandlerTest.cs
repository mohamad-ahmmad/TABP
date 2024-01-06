using Application.Abstractions;
using Application.Cities.Commands.Create;
using Application.Cities.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Cities;
public class CreateCityCommandHandlerTest 
{
    private readonly Mock<ILogger<CreateCityCommandHandler>> _logger;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<ICitiesRepository> _citiesRepo;
    private readonly Mock<IImageUploaderService> _imageUploader;
    private readonly Mock<IUserContext> _userContext;
    private readonly Mock<IMapper> _mapper;

    private readonly CreateCityCommandHandler _sut;
    public CreateCityCommandHandlerTest()
    {
        _logger = new Mock<ILogger<CreateCityCommandHandler>>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _citiesRepo = new Mock<ICitiesRepository>();
        _imageUploader = new Mock<IImageUploaderService>();
        _userContext = new Mock<IUserContext>();
        _mapper = new Mock<IMapper>();

        _imageUploader.Setup(i => i.UploadImageAsync(It.IsAny<IEnumerable<IFormFile>>()))
            .ReturnsAsync(new List<string>
            {
                "url"
            });
        _mapper.Setup(m => m.Map<CityDto>(It.IsAny<City>()))
            .Returns(new CityDto());

        _sut = new CreateCityCommandHandler
            (
                _logger.Object,
                _unitOfWork.Object,
                _citiesRepo.Object,
                _imageUploader.Object,
                _userContext.Object,
                _mapper.Object
            );
    }

    [Theory]
    [MemberData(nameof(CityAdminReturnsCases))]
    public async Task Handle_Tests(bool cityExists,
        bool isAdmin,
        IEnumerable<Error> errors,
        bool isSuccess,
        int unitOfWorkTimes,
        int imageUploaderTimes)
    {
        //Arrange
        var creatCityCommand = new CreateCityCommand(new CityForCreateDto());
        var cancellationToken = new CancellationToken();

        _citiesRepo.Setup(repo => repo.DoesCityExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cityExists);

        _userContext.Setup(u => u.GetUserLevel())
            .Returns(() =>
            {
                return isAdmin ? UserLevels.Admin : UserLevels.User;
            });

        //Act
        var result = await _sut.Handle(creatCityCommand, cancellationToken);

        //Assert
        result.IsSuccess.Should().Be(isSuccess);
        result.Errors.Should().Equal(errors);

        _unitOfWork.Verify(u=> u.CommitAsync(It.IsAny<CancellationToken>()), Times.Exactly(unitOfWorkTimes));
        _imageUploader.Verify(i=> i.UploadImageAsync(It.IsAny<IEnumerable<IFormFile>>()), Times.Exactly(imageUploaderTimes));
    }

    public static IEnumerable<object[]> CityAdminReturnsCases => new List<object[]>
        {
            new object[] { true, false, new List<Error> {CityErrors.CityAlreadyExist}, false, 0, 0},
            new object[] { false, false, new List<Error> {CityErrors.UnauthorizedToCreateCity}, false, 0, 0 },
            new object[] { false, true, new List<Error> {}, true, 1, 1},
        };
}

