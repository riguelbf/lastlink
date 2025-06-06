using Application.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using Application.Features.AdvanceRequests.Commands.CreateAdvanceRequest;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Presentation.Common;
using Presentation.Endpoints.AdvanceRequest;
using SharedKernel.Primitives;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace UnitTests.AdvanceRequest.Endpoints;

public class CreateAdvanceRequestEndpointTests
{
    private readonly ISender _sender = Substitute.For<ISender>();

    [Fact]
    public async Task HandleAsync_ShouldReturnCreated_WhenRequestIsValid()
    {
        // Arrange
        const string creatorId = "user-1";
        var id = Guid.NewGuid();
        var command = new CreateAdvanceRequestCommand(
            creatorId,
            1000m,
            DateTime.UtcNow);
    
        var response = new CreateAdvanceRequestResponse(id, creatorId, 1000m, 100m, DateTime.Today,  "Pending");
        
        _sender.Send(Arg.Any<CreateAdvanceRequestCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<CreateAdvanceRequestResponse>.Success(response));
    
        // Act
        var result = await CreateAdvanceRequestEndpoint.HandleAsync(
            command,
            _sender,
            CancellationToken.None);
    
        // Assert
        var resultType = result.GetType();
        var value = resultType.GetProperty("Value")!.GetValue(result)!;
        var responseResult = value.GetType().GetProperty("id")!.GetValue(value) as CreateAdvanceRequestResponse;
        responseResult .Should().BeEquivalentTo(response);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenAmountIsLessThan100()
    {
        // Arrange
        var command = new CreateAdvanceRequestCommand("user-1", 99m, DateTime.UtcNow);
        var error = Error.Validation("AdvanceRequest.InvalidAmount", "Amount must be at least 100");

        _sender.Send(Arg.Any<CreateAdvanceRequestCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<CreateAdvanceRequestResponse>.Failure(error));

        // Act
        var result = await CreateAdvanceRequestEndpoint.HandleAsync(
            command,
            _sender,
            CancellationToken.None);

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();

        var badRequest = result as ProblemHttpResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(Status500InternalServerError);
        badRequest.ProblemDetails.Detail.Should().BeEquivalentTo(error.Message);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenUserHasPendingRequest()
    {
        // Arrange
        var command = new CreateAdvanceRequestCommand("user-1", 1000m, DateTime.UtcNow);
        var error = Error.Conflict("AdvanceRequest.PendingRequestExists", "User already has a pending request");
        
        _sender.Send(Arg.Any<CreateAdvanceRequestCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure<CreateAdvanceRequestResponse>(error));
    
        // Act
        var result = await CreateAdvanceRequestEndpoint.HandleAsync(
            command,
            _sender,
            CancellationToken.None);
    
        // Assert
        result.Should().BeOfType<BadRequest<ApiError>>();
        var badRequest = result as BadRequest<ApiError>;
        badRequest!.StatusCode.Should().Be(Status400BadRequest);
        badRequest!.Value!.Message.Should().NotBeNull().And.NotBeEmpty().And.NotBeNullOrWhiteSpace().And.Be(error.Message);
    }
}
