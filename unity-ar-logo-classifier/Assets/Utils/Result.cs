using System;
using System.Runtime.ExceptionServices;

namespace R3
{
    public readonly struct Result<T>
    {
        public T Value { get; }
        public Exception? Exception { get; }

        public bool IsSuccess => Exception == null;
        public bool IsFailure => Exception != null;

        private Result(T value, Exception? exception)
        {
            Value = value;
            Exception = exception;
        }

        public static Result<T> Success(T value) => new Result<T>(value, null);
        public static Result<T> Failure(Exception exception) => new Result<T>(default!, exception);

        public void TryThrow()
        {
            if (IsFailure)
            {
                ExceptionDispatchInfo.Capture(Exception).Throw();
            }
        }

        public override string ToString() => IsSuccess ? $"Success({Value})" : $"Failure({Exception?.Message})";
    }
}
