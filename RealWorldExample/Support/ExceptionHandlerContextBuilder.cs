using RealWorldExample.Results;

namespace RealWorldExample.Support;

public static class ExceptionHandlerContextBuilder
{
    public static IExpectOtherExceptionBuilder Handle<TException>()
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        ExceptionHandlerContext context = new();
        context.AddExceptionPredicate((ExceptionPredicate, default));
        return new ExpectOtherError(context);
    }
}

public interface IExpectOtherExceptionBuilder
{
    IExpectOtherExceptionBuilder WithError(ErrorResult errorResult);

    IExpectOtherExceptionBuilder Handle<TException>() where TException : Exception;

    IExecuteExceptionHandlerContextBuilder WithNoMoreHandlers();
}

public abstract class ExpectErrors : IExpectOtherExceptionBuilder
{
    protected ExpectErrors(ExceptionHandlerContext context) => this.Context = context;

    protected ExceptionHandlerContext Context { get; }

    public abstract IExpectOtherExceptionBuilder Handle<TException>() where TException : Exception;

    public abstract IExpectOtherExceptionBuilder WithError(ErrorResult errorResult);

    public IExecuteExceptionHandlerContextBuilder WithNoMoreHandlers() => new ExecuteExceptionHandlerContext(this.Context);
}

public class ExpectOtherError : ExpectErrors
{
    public ExpectOtherError(ExceptionHandlerContext context) : base(context) { }

    public override IExpectOtherExceptionBuilder Handle<TException>()
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        this.Context.AddExceptionPredicate((ExceptionPredicate, default));
        return this;
    }

    public override IExpectOtherExceptionBuilder WithError(ErrorResult errorResult)
    {
        this.Context.Update(errorResult);
        return this;
    }
}

public interface IExecuteExceptionHandlerContextBuilder
{
    IExecuteExceptionHandlerContextBuilder WithDefaultError(ErrorResult errorResult);

    Result<T> Execute<T>(Func<T> action);
}

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
