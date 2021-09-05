using System;

namespace TryOperationStudy
{
    internal static class Program
    {
        private static void Main()
        {
            var operationBuilder = SomeOperation.With("contextOne");
            operationBuilder
                .Handle<InvalidOperationException>()
                .Or<ArgumentNullException>()
                .WithLogger("loggerOne2")
                .Execute(context =>
                {
                    Console.WriteLine("test" + context);
                    //throw new ArgumentException("not allowed");
                });

            Console.ReadKey();
        }
    }
}