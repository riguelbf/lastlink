using Application.Features.AdvanceRequests.Queries.GetAdvanceRequestsByCreator;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Presentation.Endpoints.AdvanceRequest;
using SharedKernel.Primitives;

namespace UnitTests.AdvanceRequest.Endpoints;

public class GetAdvanceRequestsByCreatorEndpointTests
{
    private readonly ISender _sender = Substitute.For<ISender>();
    private const string CreatorId = "user-1";
    
    [Fact]
    public async Task HandleAsync_ShouldReturnAdvanceRequests_WhenCreatorHasRequests()
    {
        // Arrange
        var requests = new List<GetAdvanceRequestsByCreatorResponse>
        {
            new(
                Guid.NewGuid(),
                1000m,
                "Pending",
                DateTime.UtcNow,
                CreatorId)
        };
    
        _sender.Send(Arg.Any<GetAdvanceRequestsByCreatorQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<IReadOnlyList<GetAdvanceRequestsByCreatorResponse>>.Success(requests));
    
        // Act
        var result = await GetAdvanceRequestsByCreatorEndpoint.HandleAsync(
            CreatorId,
            _sender,
            CancellationToken.None);
    
        // Assert
        var okResult = result as Ok<Result<IReadOnlyList<GetAdvanceRequestsByCreatorResponse>>>;
        okResult!.Value!.Value.Should().BeEquivalentTo(requests);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenCreatorHasNoRequests()
    {
        // Arrange
        _sender.Send(Arg.Any<GetAdvanceRequestsByCreatorQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<IReadOnlyList<GetAdvanceRequestsByCreatorResponse>>.Success([]));
    
        // Act
        var result = await GetAdvanceRequestsByCreatorEndpoint.HandleAsync(
            CreatorId,
            _sender,
            CancellationToken.None);
    
        // Assert
        var okResult = result as Ok<Result<IReadOnlyList<GetAdvanceRequestsByCreatorResponse>>>;
        okResult!.Value!.Value.Should().BeEmpty();
    }
}
