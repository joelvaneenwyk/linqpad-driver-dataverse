using System;
using System.Threading;
using Microsoft.CSharp;

namespace NY.Dataverse.LINQPadDriver
{
    public static class StringExtensions
    {
        private static readonly Lazy<CSharpCodeProvider> CodeProvider = new(
            () => new CSharpCodeProvider(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static string Sanitise(this string input) =>
            CodeProvider.Value.CreateValidIdentifier(string.Join("_", input.Split(" ")));
    }
}
