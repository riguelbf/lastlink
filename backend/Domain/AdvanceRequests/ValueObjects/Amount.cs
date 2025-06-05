using SharedKernel.Primitives;

namespace Domain.AdvanceRequests.ValueObjects;

/// <summary>
/// Represents a monetary amount with validation for minimum value.
/// </summary>
public sealed class Amount : ValueObject
{
    public decimal Value { get; private set; }

    private const decimal MinimumAmount = 100.00m;
    private const decimal FeePercentage = 0.05m;

    /// <summary>
    /// This class is part of a domain-driven design (DDD) approach,
    /// ensuring that monetary amounts in advance requests are always valid and consistent throughout the application.
    /// </summary>
    /// <param name="value">The monetary value to be set. Must be greater than the minimum amount.</param>
    /// <exception cref="ArgumentException">Thrown when the specified value is less than the minimum amount.</exception>
    private Amount(decimal value)
    {
        if (value < MinimumAmount)
        {
            throw new ArgumentException($"Amount must be greater than {MinimumAmount:C}", nameof(value));
        }

        Value = value;
    }

    public static Result<Amount> Create(decimal value)
    {
        try
        {
            return new Amount(value);
        }
        catch (Exception ex)
        {
            return Result.Failure<Amount>(Error.Validation("Amount.Invalid", ex.Message));
        }
    }

    public decimal CalculateNetAmount()
    {
        var fee = Value * FeePercentage;
        return Value - fee;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
