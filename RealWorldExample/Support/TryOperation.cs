using RealWorldExample.Results;

namespace RealWorldExample.Support;

public delegate Exception? ExceptionPredicate(Exception ex);

public class TryOperation
{
    private TryOperation() { }

    public TryOperation(IEnumerable<ExceptionPredicate> exceptionPredicates) => 
        this.ExceptionPredicateList.AddRange(exceptionPredicates);

    private List<ExceptionPredicate> ExceptionPredicateList { get; } = Enumerable.Empty<ExceptionPredicate>().ToList();

    public IEnumerable<ExceptionPredicate?> ExceptionPredicateCollection => this.ExceptionPredicateList;

    public void AddExceptionPredicate(ExceptionPredicate exceptionPredicate) =>
        this.ExceptionPredicateList.Add(exceptionPredicate);

    public ErrorResult ErrorResult { get; internal set; } = new("Error", "unknown error");

    public static TryOperation Handle<TException>()
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        TryOperation operation = new();
        operation.AddExceptionPredicate(ExceptionPredicate);

        return operation;
    }
}

public sealed class TryOperation<T> : TryOperation
{
    public T Value { get; set; }

    public TryOperation(T value, IEnumerable<ExceptionPredicate> exceptionPredicates) 
        : base(exceptionPredicates) => this.Value = value;
}
