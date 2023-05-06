namespace RealWorldExample.Support;

public static class ExceptionHandlerContextBuilder
{
    public static IExpectOtherExceptionBuilder Handle<TException>()
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        ExceptionHandlerContext context = new();
        context.AddExceptionPredicate((ExceptionPredicate, default));
        return new ExpectOtherError(context);
    }
}
