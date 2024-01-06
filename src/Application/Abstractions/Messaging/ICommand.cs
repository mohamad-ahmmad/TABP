using Domain.Shared;
using MediatR;

namespace Application.Abstractions.Messaging;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

