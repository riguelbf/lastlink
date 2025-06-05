namespace SharedKernel.Primitives;

/// <summary>
/// Represents the result of an operation that does not return a value.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the error if the operation failed.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error != null)
        {
            throw new ArgumentException("Cannot create a successful result with an error.", nameof(error));
        }

        if (!isSuccess && error == null)
        {
            throw new ArgumentNullException(nameof(error), "An error must be provided for a failed result.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }


    /// <summary>
    /// Matches the result to one of the provided functions based on whether the result is a success or failure.
    /// </summary>
    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

        return IsSuccess ? onSuccess() : onFailure(Error!);
    }

    /// <summary>
    /// Matches the result to one of the provided actions based on whether the result is a success or failure.
    /// </summary>
    public void Match(Action onSuccess, Action<Error> onFailure)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

        if (IsSuccess)
            onSuccess();
        else
            onFailure(Error!);
    }

    /// <summary>
    /// Creates a failure result with the specified error for a specific result type.
    /// </summary>
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
    
    /// <summary>
    /// Creates a failure result with the specified error code and message for a specific result type.
    /// </summary>
    public static Result<T> Failure<T>(string code, string message) => Result<T>.Failure(new Error(code, message));
    
    /// <summary>
    /// Creates a success result.
    /// </summary>
    public static Result Success() => new(true, null!);
}

/// <summary>
/// Represents the result of an operation that returns a value.
/// </summary>
public class Result<T> : Result
{
    /// <summary>
    /// Gets the value if the operation was successful.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    protected internal Result(bool isSuccess, T? value, Error? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a success result with the specified value.
    /// </summary>
    public static Result<T> Success(T value) => new(true, value, null);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    public static new Result<T> Failure(Error error) => new(false, default, error);

    /// <summary>
    /// Creates a failure result with the specified error code and message.
    /// </summary>
    public static new Result<T> Failure(string code, string message) => 
        new(false, default, new Error(code, message));

    /// <summary>
    /// Matches the result to one of the provided functions based on whether the result is a success or failure.
    /// </summary>
    public new TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

        return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }

    /// <summary>
    /// Matches the result to one of the provided actions based on whether the result is a success or failure.
    /// </summary>
    public new void Match(Action<T> onSuccess, Action<Error> onFailure)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

        if (IsSuccess)
            onSuccess(Value!);
        else
            onFailure(Error!);
    }

    /// <summary>
    /// Implicitly converts a value to a success result.
    /// </summary>
    public static implicit operator Result<T>(T value) => Success(value);
}