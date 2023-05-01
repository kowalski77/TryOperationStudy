using Microsoft.Extensions.Logging;
using RealWorldExample.Results;

namespace RealWorldExample.Support;

public delegate Exception? ExceptionPredicate(Exception ex);

public class TryOperation
{
    public ErrorResult? ErrorResult { get; set; }

    public string Value { get; set; }

    public TryOperation(string value) => this.Value = value;

    public ICollection<ExceptionPredicate?> ExceptionPredicateCollection { get; } = new List<ExceptionPredicate?>();

    public static TryOperation For(string value) => new(value);
}

public static class TryOperationExtensions
{
    public static TryOperation WithError(this TryOperation builder, ErrorResult errorResult)
    {
        builder.NotNull().ErrorResult = errorResult;
        return builder;
    }

    public static TryOperation Handle <TException>(this TryOperation builder)
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        builder.NotNull().ExceptionPredicateCollection.Add(ExceptionPredicate);
        return builder;
    }

    public static Result<TR> Execute<TR>(this TryOperation builder, Func<string, TR> action)
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
