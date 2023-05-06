using RealWorldExample.Results;

namespace RealWorldExample.Support;

public abstract class ExpectErrors : IExpectOtherExceptionBuilder
{
    protected ExpectErrors(ExceptionHandlerContext context) => this.Context = context;

    protected ExceptionHandlerContext Context { get; }

    public abstract IExpectOtherExceptionBuilder Handle<TException>() where TException : Exception;

    public abstract IExpectOtherExceptionBuilder WithError(ErrorResult errorResult);

    public IExecuteExceptionHandlerContextBuilder WithNoMoreHandlers() => new ExecuteExceptionHandlerContext(this.Context);
}
