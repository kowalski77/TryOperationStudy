namespace RealWorldExample.Results;

public static class ResultExtensions
{
    public static Result Validate(this Result _, params Result[] results)
    {
        List<ErrorResult> errorResults = results
            .Where(result => result.Failure)
            .SelectMany(result => result.Errors!)
            .ToList();

        return errorResults.Any() ?
            errorResults :
            Result.Ok();
    }

    public static Result<TR> Map<T, TR>(this Result<T> result, Func<T, TR> func) =>
        result.NotNull().Failure ?
            result.ToErrorsResult() :
            func.NotNull()(result.Value);

    public static Result OnSuccess(this Result result, Action action)
    {
        if (result.NotNull().Failure)
        {
            return result;
        }

        action.NotNull()();
        return Result.Ok();
    }

    public static Result<T> Bind<T>(this Result result, Func<T> func) =>
    result.NotNull().Success ?
        func.NotNull()() :
        result.ToErrorsResult();

    public static Result<T> Bind<T>(this Result result, Func<Result<T>> func) =>
    result.NotNull().Success ?
        func.NotNull()() :
        result.ToErrorsResult();

    public static Result<TR> Bind<T, TR>(this Result<T> result, Func<T, Result<TR>> func) =>
        result.NotNull().Success ?
            func.NotNull()(result.Value) :
            result.ToErrorsResult();

    public static Result Bind<T>(this Result<T> result, Func<T, Result> func) =>
    result.NotNull().Success ?
        func.NotNull()(result.Value) :
        result.ToErrorsResult();

    public static List<ErrorResult> ToErrorsResult(this Result result) =>
        result.NotNull().Errors!.ToList();
}