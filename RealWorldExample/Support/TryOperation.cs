using RealWorldExample.Results;

namespace RealWorldExample.Support;

public delegate Exception? ExceptionPredicate(Exception ex);

public class TryOperation
{
    public ErrorResult? ErrorResult { get; set; } = new("Error", "unknown error");

    public ICollection<ExceptionPredicate?> ExceptionPredicateCollection { get; internal set; } = new List<ExceptionPredicate?>();

    public static TryOperation Handle<TException>()
        where TException : Exception
    {
        TryOperation operation = new();
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        operation.ExceptionPredicateCollection.Add(ExceptionPredicate);

        return operation;
    }
}

public sealed class TryOperation<T> : TryOperation
{
    public T Value { get; set; }

    public TryOperation(T value) => this.Value = value;
}
