using RealWorldExample.Results;

namespace RealWorldExample.Support;

public interface IExpectOtherErrorsBuilder
{
    IExpectOtherErrorsBuilder WithError(ErrorResult errorResult);

    IExpectOtherErrorsBuilder Handle<TException>() where TException : Exception;

    ITryOperationHandler WithNoMoreHandlers();
}
