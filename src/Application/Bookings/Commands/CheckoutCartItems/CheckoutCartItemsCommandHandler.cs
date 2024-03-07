using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.CartItems.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Data;
using System.Net;

namespace Application.Bookings.Commands.CheckoutCartItems;
public class CheckoutCartItemsCommandHandler : ICommandHandler<CheckoutCartItemsCommand, Empty>
{
    private readonly IPaymentService _paymentService;
    private readonly IEmailService _emailService;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartItemsRepository _cartItemsRepo;
    private readonly IRoomsRepository _roomsRepo;
    private readonly IBookingsRepository _bookingsRepo;
    private readonly IMapper _mapper;
    private readonly IUsersRepository _userRepo;

    public CheckoutCartItemsCommandHandler(IPaymentService paymentService,
        IEmailService emailService,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        ICartItemsRepository cartItemsRepo,
        IRoomsRepository roomsRepository,
        IBookingsRepository bookingsRepository,
        IUsersRepository usersRepository,
        IMapper mapper
        )
    {
        _paymentService = paymentService;
        _emailService = emailService;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _cartItemsRepo = cartItemsRepo;
        _roomsRepo = roomsRepository;
        _bookingsRepo = bookingsRepository;
        _mapper = mapper;
        _userRepo = usersRepository;
    }
    public async Task<Result<Empty>> Handle(CheckoutCartItemsCommand request, CancellationToken cancellationToken)
    {
        if(request.UserId == _userContext.GetUserId())
        {
            return Result<Empty>.Failure(BookingErrors.ForbidToCheckoutCartItems, HttpStatusCode.Forbidden);
        }
        
        var cartItems = await _cartItemsRepo.GetCartItemsByUserIdAsync(request.UserId, cancellationToken);

        var userEmail = await _userRepo.GetUserEmailByUserIdAsync(request.UserId, cancellationToken);

        if(userEmail  == null)
        {
            return Result<Empty>.Failure(UserErrors.NotFoundUser, HttpStatusCode.NotFound);
        }
        
        if(!cartItems.Any())
        {
            return Result<Empty>.Failure(CartItemErrors.NotFoundCartItem, HttpStatusCode.NotFound);
        }
        
        try
        {
            await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            var invalidRooms = await _roomsRepo.GetInvalidRoomsByBookingDateTimeIntervalAsync(cartItems, cancellationToken);

            if (invalidRooms.Any())
            {
                await _unitOfWork.RollBackTransactionAsync(cancellationToken);
                
                return Result<Empty>.Failures(invalidRooms
                    .Select(room => RoomErrors.RoomWithIdIsAlreadyBookedError(room.Id))
                    .ToList(),
                    HttpStatusCode.BadRequest);
            }
            
            var cartItemsDto = _mapper.Map<IEnumerable<CartItemDto>>(cartItems);

            var bookings = await AddBookingsFromCartItemsDtoAsync(cartItemsDto, cancellationToken);

            var amoutMoney = GetAmoutMoney(cartItemsDto);

            var emailMessage = GenerateEmailMessage(bookings);

            await _emailService.SendEmailAsync(userEmail, $"Invoice", emailMessage);
            

            var result = await _paymentService.PayAsync(request.CardDetailsToken,
                request.IdempotencyKey,
                amoutMoney,
                "USD");



            if (result.IsFailure)
            {
                await _unitOfWork.RollBackTransactionAsync(cancellationToken);
                return result;
            }


            await _unitOfWork.CommitTransationAsync(cancellationToken);

            return Result<Empty>.Success(Empty.Value)!;
        }
        catch
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);
            throw;
        }


    }

    private static string GenerateEmailMessage(IEnumerable<Booking> bookings)
    {
        var emailMessage = string.Join(Environment.NewLine,
            bookings.Select(b =>
                $"Invoice Id: {b.Id}\n" +
                $"From Date: {b.FromDate}\n" +
                $"To Date: {b.ToDate}\n" +
                $"Price Per Day: {b.PricePerDay}\n" +
                $"Room Id: {b.RoomId}\n" +
                $"Discount Percentage: {b.DiscountPercentage}\n" +
                $"------------------------------------------------------------"
                )
            );

        return emailMessage;
    }

    private async Task<IEnumerable<Booking>> AddBookingsFromCartItemsDtoAsync(IEnumerable<CartItemDto> cartItemsDto, CancellationToken cancellationToken)
    {
        var bookings = new List<Booking>();
        foreach (var cartItem in cartItemsDto)
        {
            var booking = new Booking
            {
                DiscountPercentage = cartItem.DiscountPercentage,
                FromDate = cartItem.FromDate,
                ToDate = cartItem.ToDate,
                HasRated = false,
                PricePerDay = cartItem.PricePerDay,
                RoomId = cartItem.RoomId,
                UserId = cartItem.UserId,
            };
            await _bookingsRepo.AddBookingAsync(booking, cancellationToken);
            bookings.Add(booking);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
        return bookings;
    }

    private static double GetAmoutMoney(IEnumerable<CartItemDto> cartItemsDto)
    {
        return cartItemsDto.Sum(ci => (ci.ToDate - ci.FromDate).Days*(ci.PricePerDay - (ci.PricePerDay * (ci.DiscountPercentage / 100))));
    }
}
