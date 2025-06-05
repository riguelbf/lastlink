using Application.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Presentation.Endpoints.AdvanceRequest;
using SharedKernel.Primitives;

namespace UnitTests.AdvanceRequest.Enpoints;

public class ApproveAdvanceRequestEndpointTests
{
    private readonly ISender _sender = Substitute.For<ISender>();
    private readonly Guid _requestId = Guid.NewGuid();
    private readonly DateTime _actionDate = DateTime.Today;

    [Fact]
    public async Task HandleAsync_ShouldApproveRequest_WhenValid()
    {
        // Arrange
        _sender.Send(Arg.Any<ApproveAdvanceRequestCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());

        // Act
        var result = await ApproveAdvanceRequestEndpoint.HandleAsync(
            _requestId,
            _sender,
            CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContent>();
        await _sender.Received(1).Send(
            Arg.Is<ApproveAdvanceRequestCommand>(c => 
                c.RequestId == _requestId && 
                c.ActionDate == _actionDate), 
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenRequestDoesNotExist()
    {
        // Arrange
        var error = Error.NotFound("NotFound", "The advance request was not found");
        _sender.Send(Arg.Any<ApproveAdvanceRequestCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure<NotFound>(error));

        // Act
        var result = await ApproveAdvanceRequestEndpoint.HandleAsync(
            _requestId,
            _sender,
            CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFound<(string, string)>>();
        var notFoundResult = result as NotFound<(string, string)>;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be((error.Code, error.Message));
    }
}
