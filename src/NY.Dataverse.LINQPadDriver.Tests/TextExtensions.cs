using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using JetBrains.Annotations;

namespace NY.Dataverse.LINQPadDriver.Tests;

[PublicAPI]
internal static class TextExtensions
{
    public static string JoinNewLine(this string? first, params string?[] other)
    {
        return JoinNewLine(other.Prepend(first));
    }

    private static string JoinNewLine(this IEnumerable<string?> other)
    {
        return string.Join(Environment.NewLine, other);
    }

    public static string Pluralize<T>(this IReadOnlyCollection<T> collection, string what, string? fallback = null)
    {
        return collection.Count > 1
            ? fallback ?? what.Pluralize()
            : what;
    }

#if false
    public static string AppendDot(this string str) =>
        AppendDotRegex().IsMatch(str)
            ? str
            : str + ".";
#endif

    public static string ReplaceHotKeyChar(this string str, string? newChar = null)
    {
        return str.Replace("_", newChar);
    }
}