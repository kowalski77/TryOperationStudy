namespace RealWorldExample.Support;

public static class TryOperationBuilder
{
    public static IExpectOtherErrorsBuilder Handle<TException>()
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
        TryOperation context = new();
        context.AddExceptionPredicate((ExceptionPredicate, default));
        return new ExpectOtherError(context);
    }
}
