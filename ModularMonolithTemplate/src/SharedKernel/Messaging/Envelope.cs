using System.Diagnostics;

namespace ModularMonolithTemplate.SharedKernel.Messaging;

public sealed record Envelope<T>(
    T Message,
    string TraceParent,
    string? TraceState,
    IReadOnlyDictionary<string, string>? Baggage);

public static class Envelope
{
    public static Envelope<T> FromCurrent<T>(T msg)
    {
        var parent = Activity.Current?.Id ?? string.Empty;
        var state = Activity.Current?.TraceStateString;
        var bag = Activity.Current?.Baggage?.ToDictionary(kv => kv.Key, kv => kv.Value) ?? new Dictionary<string, string>();
        return new Envelope<T>(msg, parent, state, bag);
    }
}
