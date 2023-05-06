using RealWorldExample.Results;

namespace RealWorldExample.Support;

public interface IExecuteExceptionHandlerContextBuilder
{
    IExecuteExceptionHandlerContextBuilder WithDefaultError(ErrorResult errorResult);

    Result<T> Execute<T>(Func<T> action);
}
