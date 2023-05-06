﻿using RealWorldExample.Results;

namespace RealWorldExample.Support;

public delegate Exception? ExceptionPredicate(Exception ex);

public class TryOperation
{
    private TryOperation() { }

    public TryOperation(IEnumerable<(ExceptionPredicate, ErrorResult?)> exceptionPredicates) => 
        this.ExceptionPredicateList.AddRange(exceptionPredicates);

    private List<(ExceptionPredicate, ErrorResult?)> ExceptionPredicateList { get; } =
        Enumerable.Empty<(ExceptionPredicate, ErrorResult?)>().ToList();

    public IEnumerable<(ExceptionPredicate, ErrorResult?)> ExceptionPredicateCollection => 
        this.ExceptionPredicateList;

    public void AddExceptionPredicate((ExceptionPredicate, ErrorResult?) exceptionPredicate) =>
        this.ExceptionPredicateList.Add(exceptionPredicate);

    public ErrorResult ErrorResult { get; internal set; } = new("Error", "unknown error");

    public static TryOperation Handle<TException>()
    where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        TryOperation operation = new();
        operation.AddExceptionPredicate((ExceptionPredicate, default));

        return operation;
    }

    internal void Update(ErrorResult errorResult)
    {
        (ExceptionPredicate predicate, ErrorResult? _) = this.ExceptionPredicateList.Last();
        this.ExceptionPredicateList[^1] = (predicate, errorResult);
    }
}

public sealed class TryOperation<T> : TryOperation
{
    public T Value { get; set; }

    public TryOperation(T value, IEnumerable<(ExceptionPredicate, ErrorResult?)> exceptionPredicates) 
        : base(exceptionPredicates) => this.Value = value;
}
