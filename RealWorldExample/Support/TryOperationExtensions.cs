using RealWorldExample.Results;

namespace RealWorldExample.Support;

public static class TryOperationExtensions
{
    public static TryOperation<T> For<T>(this TryOperation tryOperation, T value)
    {
        TryOperation<T> newlyTryOperation = new(value)
        {
            ExceptionPredicateCollection = tryOperation.NotNull().ExceptionPredicateCollection
        };
        return newlyTryOperation;
    }

    public static TryOperation<T> WithError<T>(this TryOperation<T> builder, ErrorResult errorResult)
    {
        builder.NotNull().ErrorResult = errorResult;
        return builder;
    }

    public static Result<TR> Try<T, TR>(this TryOperation<T> builder, Func<T, TR> action)
    {
        try
        {
            return action.NotNull()(builder.NotNull().Value);
        }
        catch (Exception e)
        {
            ExceptionPredicate? exception = builder.NotNull().ExceptionPredicateCollection.FirstOrDefault(x => x?.Invoke(e) is not null);
            if (exception is null)
            {
                throw;
            }

            return builder.ErrorResult;
        }
    }
}
