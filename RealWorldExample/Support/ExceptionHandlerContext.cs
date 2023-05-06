using RealWorldExample.Results;

namespace RealWorldExample.Support;

public delegate Exception? ExceptionPredicate(Exception ex);

public class ExceptionHandlerContext
{
    internal ExceptionHandlerContext() { }

    public ExceptionHandlerContext(IEnumerable<(ExceptionPredicate, ErrorResult?)> exceptionPredicates) =>
        this.ExceptionPredicateList.AddRange(exceptionPredicates);

    private List<(ExceptionPredicate, ErrorResult?)> ExceptionPredicateList { get; } = 
        Enumerable.Empty<(ExceptionPredicate, ErrorResult?)>().ToList();

    public IEnumerable<(ExceptionPredicate, ErrorResult?)> ExceptionPredicateCollection =>
        this.ExceptionPredicateList;

    public void AddExceptionPredicate((ExceptionPredicate, ErrorResult?) exceptionPredicate) =>
        this.ExceptionPredicateList.Add(exceptionPredicate);

    public ErrorResult ErrorResult { get; internal set; } = new("Error", "unknown error");

    internal void Update(ErrorResult errorResult)
    {
        (ExceptionPredicate predicate, ErrorResult? _) = this.ExceptionPredicateList.Last();
        this.ExceptionPredicateList[^1] = (predicate, errorResult);
    }
}
