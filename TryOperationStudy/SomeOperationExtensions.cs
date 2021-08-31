using System;
using System.Linq;

namespace TryOperationStudy
{
    public static class SomeOperationExtensions
    {
        public static SomeOperation Handle<TException>(this SomeOperation builder)
            where TException : Exception
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            static Exception? ExceptionPredicate(Exception exception) => exception is TException ? exception : null;
            builder.ExceptionPredicateCollection.Add(ExceptionPredicate);

            return builder;
        }

        public static SomeOperation Or<TException>(this SomeOperation builder)
            where TException : Exception
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.Handle<TException>();
        }

        public static SomeOperation WithLogger(this SomeOperation builder, string logger)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Logger = logger;

            return builder;
        }

        public static void Execute(this SomeOperation builder, Action<string> action)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (action == null) throw new ArgumentNullException(nameof(action));

            try
            {
                Console.WriteLine("Logger in action: " + builder.Logger);
                action(builder.Context);
            }
            catch (Exception e)
            {
                var exception = builder.ExceptionPredicateCollection.FirstOrDefault(x => x?.Invoke(e) is not null);
                if (exception is null)
                {
                    throw;
                }

                Console.WriteLine("Exception handled: " + e.GetType().Name);
            }
        }
    }
}