using RealWorldExample.Results;

namespace RealWorldExample.Support;

public delegate Exception? ExceptionPredicate(Exception ex);

public class TryOperation
{
    public ErrorResult? ErrorResult { get; set; }

    public ICollection<ExceptionPredicate?> ExceptionPredicateCollection { get; } = new List<ExceptionPredicate?>();

    public static TryOperation<T> For<T>(T value) => new(value);
}

public sealed class TryOperation<T> : TryOperation
{
    public T Value { get; set; }

    public TryOperation(T value) => this.Value = value;
}

public static class TryOperationExtensions
{
    public static TryOperation<T> WithError<T>(this TryOperation<T> builder, ErrorResult errorResult)
    {
        builder.NotNull().ErrorResult = errorResult;
        return builder;
    }

    public static TryOperation<T> Handle<T, TException>(this TryOperation<T> builder)
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        builder.NotNull().ExceptionPredicateCollection.Add(ExceptionPredicate);
        return builder;
    }

    public static Result<TR> Execute<T, TR>(this TryOperation<T> builder, Func<T, TR> action)
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

            return builder.ErrorResult ?? new ErrorResult("Error", "unknown error");
        }
    }
}
