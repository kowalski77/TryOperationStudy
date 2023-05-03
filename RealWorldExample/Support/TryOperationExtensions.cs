using RealWorldExample.Results;

namespace RealWorldExample.Support;

public static class TryOperationExtensions
{
    public static TryOperation Handle<TException>(this TryOperation operation)
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        operation.NotNull().AddExceptionPredicate(ExceptionPredicate);

        return operation;
    }

    public static TryOperation<T> For<T>(this TryOperation operation, T value) =>
        new(value, operation.NotNull().ExceptionPredicateCollection!);

    public static TryOperation<T> WithError<T>(this TryOperation<T> operation, ErrorResult errorResult)
    {
        operation.NotNull().ErrorResult = errorResult;
        return operation;
    }

    public static Result<TR> Try<T, TR>(this TryOperation<T> operation, Func<T, TR> action)
    {
        try
        {
            return action.NotNull()(operation.NotNull().Value);
        }
        catch (Exception e)
        {
            ExceptionPredicate? exception = operation.NotNull().ExceptionPredicateCollection.FirstOrDefault(x => x?.Invoke(e) is not null);
            if (exception is null)
            {
                throw;
            }

            return operation.ErrorResult;
        }
    }
}
