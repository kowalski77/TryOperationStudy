using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System;

namespace TryOperationStudy;

public static class ArgumentExtensions
{
    public static T NonNull<T>([NotNull] this T value, [CallerArgumentExpression(nameof(value))] string? param = null) =>
        value ??
        throw new ArgumentNullException(param);
}
