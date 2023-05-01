namespace RealWorldExample.Results;

public class Result
{
    protected Result(IEnumerable<ErrorResult> errorResults) => Errors = errorResults.ToList();

    protected Result(ErrorResult errorResult) => Errors = new List<ErrorResult> { errorResult };

    protected Result() => Success = true;

    public IEnumerable<ErrorResult>? Errors { get; }

    public bool Success { get; }

    public bool Failure => !Success;

    public static Result Init => Ok();

    public static Result Ok() => new();

    public static Result<T> Ok<T>(T value) => new(value);

    public static Result Fail(IEnumerable<ErrorResult> errorResults) => new(errorResults);

    public static Result<T> Fail<T>(IEnumerable<ErrorResult> errorResults) => new(errorResults);

    public static implicit operator Result(ErrorResult error) => new(error);

    public static implicit operator Result(List<ErrorResult> errors) => new(errors);
}
