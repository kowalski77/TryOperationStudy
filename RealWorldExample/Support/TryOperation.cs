using RealWorldExample.Results;

namespace RealWorldExample.Support;

public delegate Exception? ExceptionPredicate(Exception ex);

public class TryOperation
{
    public TryOperation(IEnumerable<ExceptionPredicate?> exceptionPredicateCollection) =>
        this.ExceptionPredicateCollection = exceptionPredicateCollection.ToList();

    public ErrorResult ErrorResult { get; internal set; } = new("Error", "unknown error");

    public IReadOnlyList<ExceptionPredicate?> ExceptionPredicateCollection { get; }

    public static TryOperation Handle<TException>()
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        TryOperation operation = new(new List<ExceptionPredicate> { ExceptionPredicate });

        return operation;
    }

    // TODO: append another exception


}

public sealed class TryOperation<T> : TryOperation
{
    public T Value { get; set; }

    public TryOperation(T value, IEnumerable<ExceptionPredicate> exceptionPredicates) 
        : base(exceptionPredicates) => this.Value = value;
}
