using System;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    protected Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, string.Empty);

    public static Result<T> Failure(string error) => new Result<T>(false, default(T), error);

    // Implicit conversion operators can be added if needed, e.g.:
    // public static implicit operator Result<T>(T value) => Success(value);
}

// Optional: Non-generic version for operations without a return value
public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, string.Empty);

    public static Result Failure(string error) => new Result(false, error);
}
