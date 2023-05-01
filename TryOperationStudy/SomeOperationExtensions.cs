using System;
using System.Linq;

namespace TryOperationStudy;

public delegate Exception? ExceptionPredicate(Exception ex);

public static class SomeOperationExtensions
{
    public static SomeOperation Handle<TException>(this SomeOperation builder)
        where TException : Exception
    {
        static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;

        builder.NonNull().ExceptionPredicateCollection.Add(ExceptionPredicate);

        return builder;
    }

    public static SomeOperation Or<TException>(this SomeOperation builder)
        where TException : Exception => 
        builder.NonNull().Handle<TException>();

    public static SomeOperation WithLogger(this SomeOperation builder, string logger)
    {
        builder.NonNull().Logger = logger;

        return builder;
    }

    public static void Execute(this SomeOperation builder, Action<string> action)
    {
        try
        {
            Console.WriteLine("Logger in action: " + builder.NonNull().Logger);
            action.NonNull()(builder.Context);
        }
        catch (Exception e)
        {
            ExceptionPredicate? exception = builder!.ExceptionPredicateCollection.FirstOrDefault(x => x?.Invoke(e) is not null);
            if (exception is null)
            {
                throw;
            }

            Console.WriteLine("Exception handled: " + e.GetType().Name);
        }
    }
}