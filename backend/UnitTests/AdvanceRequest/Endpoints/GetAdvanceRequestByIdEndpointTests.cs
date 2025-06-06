using Application.Features.AdvanceRequests.Queries.GetAdvanceRequestById;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Presentation.Common;
using Presentation.Endpoints.AdvanceRequest;
using SharedKernel.Primitives;

namespace UnitTests.AdvanceRequest.Endpoints;

public class GetAdvanceRequestByIdEndpointTests
{
    private readonly ISender _sender = Substitute.For<ISender>();
    private readonly Guid _requestId = Guid.NewGuid();
    private const string CreatorId = "user-1";

    [Fact]
    public async Task HandleAsync_ShouldReturnAdvanceRequest_WhenExists()
    {
        // Arrange
        var query = new GetAdvanceRequestByIdQuery(_requestId);
        var response = new GetAdvanceRequestByIdResponse(
            _requestId,
            1000m,
            "Pending",
            DateTime.UtcNow,
            CreatorId);
    
        _sender.Send(Arg.Any<GetAdvanceRequestByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetAdvanceRequestByIdResponse>.Success(response));
    
        // Act
        var result = await GetAdvanceRequestEndpoint.HandleAsync(
            _requestId,
            _sender,
            CancellationToken.None);
    
        // Assert
        result.Should().BeOfType<Ok<GetAdvanceRequestByIdResponse>>();
        var okResult = result as Ok<GetAdvanceRequestByIdResponse>;
        okResult!.Value.Should().BeEquivalentTo(response);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenRequestDoesNotExist()
    {
        // Arrange
        var expectedError = Error.NotFound("NotFound", "The specified advance request was not found.");

        _sender.Send(Arg.Any<GetAdvanceRequestByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetAdvanceRequestByIdResponse>.Failure(expectedError));

        // Act
        var result = await GetAdvanceRequestEndpoint.HandleAsync(
            _requestId,
            _sender,
            CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFound<ApiError>>();

        var notFound = result as NotFound<ApiError>;
        notFound!.Value!.Code.Should().Be("NotFound");
        notFound.Value.Message.Should().Be("The specified advance request was not found.");
    }
}
