using Application.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Presentation.Common;
using Presentation.Endpoints.AdvanceRequest;
using SharedKernel.Primitives;

namespace UnitTests.AdvanceRequest.Endpoints;

public class RejectAdvanceRequestEndpointTests
{
    private readonly ISender _sender = Substitute.For<ISender>();
    private readonly Guid _requestId = Guid.NewGuid();
    private readonly DateTime _actionDate = DateTime.Today;
    
    [Fact]
    public async Task HandleAsync_ShouldRejectRequest_WhenValid()
    {
        // Arrange
        _sender.Send(Arg.Any<RejectAdvanceRequestCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
    
        // Act
        var result = await RejectAdvanceRequestEndpoint.HandleAsync(
            _requestId,
            _sender,
            CancellationToken.None);
    
        // Assert
        result.Should().BeOfType<NoContent>();
        await _sender.Received(1).Send(
            Arg.Is<RejectAdvanceRequestCommand>(c => 
                c.RequestId == _requestId && 
                c.ActionDate.Date == _actionDate), 
            Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenRequestDoesNotExist()
    {
        // Arrange
        var expectedError = Error.NotFound("AdvanceRequest.NotFound", "The specified advance request was not found.");
        _sender.Send(Arg.Any<RejectAdvanceRequestCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<UpdateAdvanceRequestStatusCommand>.Failure(expectedError));
    
        // Act
        var result = await RejectAdvanceRequestEndpoint.HandleAsync(
            _requestId,
            _sender,
            CancellationToken.None);
    
        // Assert
        result.Should().BeOfType<NotFound<ApiError>>();
        var notFound = result as NotFound<ApiError>;
        notFound!.Value!.Code.Should().Be("AdvanceRequest.NotFound");
        notFound.Value.Message.Should().Be("The specified advance request was not found.");
    }
}
