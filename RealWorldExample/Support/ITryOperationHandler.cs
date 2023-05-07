using RealWorldExample.Results;

namespace RealWorldExample.Support;

public interface ITryOperationHandler
{
    ITryOperationHandler WithDefaultError(ErrorResult errorResult);

    Result<T> Execute<T>(Func<T> action);
}
