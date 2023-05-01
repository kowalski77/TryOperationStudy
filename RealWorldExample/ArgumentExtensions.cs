using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RealWorldExample;

public static class ArgumentExtensions
{
    public static T NotNull<T>([NotNull] this T value, [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        value is not null ?
        value :
        throw new ArgumentNullException(paramName);
}
