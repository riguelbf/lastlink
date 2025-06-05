using System;

namespace SharedKernel.Primitives;

/// <summary>
/// Represents an error in the domain.
/// </summary>
public class Error
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public Error(string code, string message)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    /// <summary>
    /// Creates a new error for validation failures.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <returns>A new error.</returns>
    public static Error Validation(string code, string message) => new(code, message);

    /// <summary>
    /// Creates a new error for not found failures.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <returns>A new error.</returns>
    public static Error NotFound(string code, string message) => new(code, message);

    /// <summary>
    /// Creates a new error for conflict failures.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <returns>A new error.</returns>
    public static Error Conflict(string code, string message) => new(code, message);

    /// <summary>
    /// Creates a new error for unauthorized access.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <returns>A new error.</returns>
    public static Error Unauthorized(string code, string message) => new(code, message);
}
