using Domain.Shared;
using MediatR;

namespace Application.Messaging;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

