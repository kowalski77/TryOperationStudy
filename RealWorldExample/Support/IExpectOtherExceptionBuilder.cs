using RealWorldExample.Results;

namespace RealWorldExample.Support;

public interface IExpectOtherExceptionBuilder
{
    IExpectOtherExceptionBuilder WithError(ErrorResult errorResult);

    IExpectOtherExceptionBuilder Handle<TException>() where TException : Exception;

    IExecuteExceptionHandlerContextBuilder WithNoMoreHandlers();
}
