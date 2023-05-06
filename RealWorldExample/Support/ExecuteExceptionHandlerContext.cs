using RealWorldExample.Results;

namespace RealWorldExample.Support;

public class ExecuteExceptionHandlerContext : IExecuteExceptionHandlerContextBuilder
{
    public ExecuteExceptionHandlerContext(ExceptionHandlerContext context) => this.context = context;

    private ExceptionHandlerContext context { get; }

    public IExecuteExceptionHandlerContextBuilder WithDefaultError(ErrorResult errorResult)
    {
        this.context.ErrorResult = errorResult;
        return this;
    }

    public Result<T> Execute<T>(Func<T> action)
    {
        try
        {
            return action.NotNull()();
        }
        catch (Exception e)
        {
            (ExceptionPredicate exceptionPredicate, ErrorResult? error) = context.NotNull().ExceptionPredicateCollection.FirstOrDefault(x => x.Item1?.Invoke(e) is not null);
            if (exceptionPredicate is null)
            {
                throw;
            }

            return error ?? context.ErrorResult;
        }
    }
}
