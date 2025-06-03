using FluentValidation;

namespace Presentation.Endpoints;

public abstract class EndpointBase : IEndpoint
{
    public abstract void MapEndpoint(IEndpointRouteBuilder app);
    
    /// <summary>
    /// Validates the request using the given validator. If the request is valid, this method returns a tuple where the first element is true and the second element is null.
    /// If the request is not valid, this method returns a tuple where the first element is false and the second element is an <see cref="IResult"/> representing
    /// a 400 Bad Request response with a JSON body containing information about the validation errors.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request to be validated.</typeparam>
    /// <param name="request">The request to be validated.</param>
    /// <param name="validator">The validator to be used to validate the request.</param>
    /// <returns>A tuple where the first element is true if the request is valid and false otherwise, and the second element is an <see cref="IResult"/> representing
    /// a 400 Bad Request response with a JSON body containing information about the validation errors if the request is not valid, or null if the request is valid.</returns>
    protected static async Task<(bool requestIsValid, IResult? validationResult)> ValidateRequestAsync<TRequest>(
        TRequest request,
        IValidator<TRequest> validator)
    {
        var result = await validator.ValidateAsync(request);

        if (result.IsValid) return (true, null);
        
        var errorResponse = Results.BadRequest(new
        {
            message = "Validation failed",
            errors = result.Errors.Select(e => new
            {
                field = e.PropertyName,
                error = e.ErrorMessage,
                value = e.AttemptedValue
            })
        });

        return (false, errorResponse);
    }
}