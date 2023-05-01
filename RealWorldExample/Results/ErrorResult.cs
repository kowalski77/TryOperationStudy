namespace RealWorldExample.Results;

public record ErrorResult
{
    public ErrorResult(
        string field,
        string message)
    {
        Field = field;
        Message = message;
    }

    public string Field { get; }

    public string Message { get; }

    public string? StackTrace { get; init; }
}
