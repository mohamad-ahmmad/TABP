using AutoMapper.Internal;
using Domain.Shared;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace Application.Behaviors.Validation;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {

        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(vr => vr.Errors)
            .Where(e => e != null)
            .Select(validationError => new Error(validationError.PropertyName, validationError.ErrorMessage))
            .ToList();
        
        if (failures.Any())
        {
            return ErrorsResult(failures);
        }
        
        return await next();
    }
    
    private TResponse ErrorsResult(List<Error> failures)
    {
        var res = (TResponse)(typeof(TResponse)
            .GetMethod("Failures", new Type[] {typeof(IEnumerable<Error>)})!
            .Invoke(null, new object[] {failures}))!;
        return res;
    }
}

