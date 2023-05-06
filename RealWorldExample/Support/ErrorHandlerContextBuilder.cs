﻿using RealWorldExample.Results;

namespace RealWorldExample.Support;

public static class ErrorHandlerContextBuilder
{
    public static IExpectOtherErrorBuilder Handle<TException>()
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        ErrorHandlerContext context = new();
        context.AddExceptionPredicate((ExceptionPredicate, default));
        return new ExpectOtherError(context);
    }
}

public interface IExpectOtherErrorBuilder
{
    IExpectOtherErrorBuilder Handle<TException>() where TException : Exception;

    IExecuteErrorHandlerContextBuilder WithNoMoreHandlers();
}

public abstract class ExpectErrors : IExpectOtherErrorBuilder
{
    protected ExpectErrors(ErrorHandlerContext context) => this.Context = context;

    protected ErrorHandlerContext Context { get; }

    public abstract IExpectOtherErrorBuilder Handle<TException>() where TException : Exception;

    public IExecuteErrorHandlerContextBuilder WithNoMoreHandlers() => new ExecuteErrorHandlerContext(this.Context);
}

public class ExpectOtherError : ExpectErrors
{
    public ExpectOtherError(ErrorHandlerContext context) : base(context) { }

    public override IExpectOtherErrorBuilder Handle<TException>()
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        this.Context.AddExceptionPredicate((ExceptionPredicate, default));
        return this;
    }
}

public interface IExecuteErrorHandlerContextBuilder
{
    IExecuteErrorHandlerContextBuilder WithDefaultError(ErrorResult errorResult);

    Result<T> Execute<T>(Func<T> action);
}

public abstract class ExecuteErrorsHandler : IExecuteErrorHandlerContextBuilder
{
    public abstract IExecuteErrorHandlerContextBuilder WithDefaultError(ErrorResult errorResult);

    public abstract Result<T> Execute<T>(Func<T> action);
}

public class ExecuteErrorHandlerContext : ExecuteErrorsHandler
{
    public ExecuteErrorHandlerContext(ErrorHandlerContext context) => this.context = context;

    private ErrorHandlerContext context { get; }

    public override IExecuteErrorHandlerContextBuilder WithDefaultError(ErrorResult errorResult)
    {
        this.context.ErrorResult = errorResult;
        return this;
    }

    public override Result<T> Execute<T>(Func<T> action)
    {
        try
        {
            return action.NotNull()();
        }
        catch (Exception e)
        {
            (ExceptionPredicate exceptionPredicate, ErrorResult? error) = context.NotNull().ExceptionPredicateCollection.FirstOrDefault(x => x.Item1?.Invoke(e) is not null);
            if (exceptionPredicate is null)
            {
                throw;
            }

            return error ?? context.ErrorResult;
        }
    }
}
