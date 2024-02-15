using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Discounts.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Discounts.Command.Create;
public class CreateDiscountCommandHandler : ICommandHandler<CreateDiscountCommand, DiscountDto>
{
    private readonly IDiscountsRepository _discountsRepo;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiscountCommandHandler(IDiscountsRepository discountsRepo,
        IUserContext userContext,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _discountsRepo = discountsRepo;
        _userContext = userContext;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<DiscountDto>> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return DiscountErrors.ForbidToCreateDiscount;
        }

        var discount = _mapper.Map<Discount>(request.DiscountDto);

        await _discountsRepo.AddDiscountAsync(discount, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<DiscountDto>(discount);
    }
}
