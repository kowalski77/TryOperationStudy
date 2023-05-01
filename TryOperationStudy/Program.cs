using System;
using TryOperationStudy;

SomeOperation operationBuilder = SomeOperation.With("contextOne");
operationBuilder
    .Handle<InvalidOperationException>()
    .Or<ArgumentNullException>()
    .WithLogger("loggerOne2")
    .Execute(context =>
    {
        Console.WriteLine("test" + context);
        throw new InvalidOperationException("not allowed");
    });

Console.ReadKey();