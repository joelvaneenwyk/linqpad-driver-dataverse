using System;

// ReSharper disable UnusedMember.Global

namespace LPRun;

/// <summary>
/// Defines LPRun command-line options.
/// </summary>
/// <seealso href="https://www.linqpad.net/lprun.aspx">LINQPad Command-Line and Scripting</seealso>
public static class Options
{
    /// <summary>
    /// Defines LPRun command-line compilation options.
    /// </summary>
    public static class Compilation
    {
        /// <summary>
        /// Switch enables compiler optimizations. This incurs the usual trade-off: slightly faster execution with compute-intensive code in exchange for less accurate error reporting.
        /// </summary>
        public const string Optimize        = "-optimize";

        /// <summary>
        /// Switch outputs compiler warnings. Warnings are written to stderr <see cref="Console.Error"/>.
        /// </summary>
        /// <remarks>As warnings are written to stderr LPRun execution will fail in case of any.</remarks>
        public const string OutputWarnings  = "-warn";

        /// <summary>
        /// Switch tells LPRun to check that the query will compile, without actually running anything.
        /// </summary>
        public const string CompileOnly      = "-compileonly";
    }

    /// <summary>
    /// Defines NuGet options.
    /// </summary>
    public static class NuGet
    {
        /// <summary>
        /// Switch forces NuGet download (update).
        /// </summary>
        /// <remarks>
        /// LPRrun will automatically download the required NuGet packages and dependencies upon first execution.
        /// This will work whether or not you have a registered edition of LINQPad.
        /// </remarks>
        public const string ForceDownload    = "-nunuget";
    }

    /// <summary>
    /// Defines script language.
    /// </summary>
    public static class Lang
    {
        private const string Switch          = "-lang=";

        /// <summary>
        /// Script is C# expression.
        /// </summary>
        public const string Expression       = $"{Switch}Expression";

        /// <summary>
        /// Script is C# statements.
        /// </summary>
        public const string Statements       = $"{Switch}Statements";

        /// <summary>
        /// Script is C# program.
        /// </summary>
        public const string Program          = $"{Switch}Program";

        /// <summary>
        /// Script is VB expression.
        /// </summary>
        public const string VBExpression     = $"{Switch}VBExpression";

        /// <summary>
        /// Script is VB statements.
        /// </summary>
        public const string VBStatements     = $"{Switch}VBStatements";

        /// <summary>
        /// Script is VB program.
        /// </summary>
        public const string VBProgram        = $"{Switch}VBProgram";

        /// <summary>
        /// Script is F# expression.
        /// </summary>
        public const string FSharpExpression = $"{Switch}FSharpExpression";

        /// <summary>
        /// Script is F# program.
        /// </summary>
        public const string FSharpProgram    = $"{Switch}FSharpProgram";

        /// <summary>
        /// Script is SQL.
        /// </summary>
        public const string SQL              = $"{Switch}SQL";

        /// <summary>
        /// Script is ESQL.
        /// </summary>
        public const string ESQL             = $"{Switch}ESQL";
    }
}
