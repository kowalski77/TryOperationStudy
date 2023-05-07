using RealWorldExample.Results;

namespace RealWorldExample.Support;

public abstract class ExpectErrors : IExpectOtherErrorsBuilder
{
    protected ExpectErrors(TryOperation context) => this.Context = context;

    protected TryOperation Context { get; }

    public abstract IExpectOtherErrorsBuilder Handle<TException>() where TException : Exception;

    public abstract IExpectOtherErrorsBuilder WithError(ErrorResult errorResult);

    public ITryOperationHandler WithNoMoreHandlers() => new TryOperationHandler(this.Context);
}
