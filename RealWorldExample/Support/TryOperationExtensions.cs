using RealWorldExample.Results;

namespace RealWorldExample.Support;

public static class TryOperationExtensions
{
    public static TryOperation Handle<TException>(this TryOperation operation)
        where TException : Exception => operation.Handle<TException>(default);

    public static TryOperation Handle<TException>(this TryOperation operation, ErrorResult? errorResult)
    where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        operation.NotNull().AddExceptionPredicate((ExceptionPredicate, errorResult));

        return operation;
    }

    public static TryOperation<T> For<T>(this TryOperation operation, T value) =>
        new(value, operation.NotNull().ExceptionPredicateCollection!);

    public static TryOperation<T> WithDefaultError<T>(this TryOperation<T> operation, ErrorResult errorResult)
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
            (ExceptionPredicate exceptionPredicate, ErrorResult? error) = operation.NotNull().ExceptionPredicateCollection.FirstOrDefault(x => x.Item1?.Invoke(e) is not null);
            if (exceptionPredicate is null)
            {
                throw;
            }

            return error ?? operation.ErrorResult;
        }
    }
}
