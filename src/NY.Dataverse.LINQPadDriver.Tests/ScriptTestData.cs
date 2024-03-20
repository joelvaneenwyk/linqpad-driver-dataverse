using System;
using System.Threading;
using JetBrains.Annotations;
using LPRun;

namespace NY.Dataverse.LINQPadDriver.Tests
{
    /// <summary>
    /// </summary>
    public sealed record ScriptTestData
    {
        /// <summary>
        /// Exit code when run is successful.
        /// </summary>
        public const int SuccessExitCode = 0;

        /// <summary>
        /// Exit code to use if there is an error.
        /// </summary>
        public static readonly int ErrorExitCode = new Random().Next() % 250 + 2;

        /// <summary>
        /// Unique identifier for success messages
        /// </summary>
        public static readonly Guid SuccessMessage = Guid.NewGuid();

        /// <summary>
        /// Unique identifier for error messages
        /// </summary>
        public static readonly Guid ErrorMessage = Guid.NewGuid();

        /// <summary>
        /// </summary>
        /// <param name="Succeeded"></param>
        /// <param name="Payload"></param>
        /// <param name="HasErrorOutput"></param>
        /// <param name="ExitCode"></param>
        public ScriptTestData(
            bool Succeeded,
            string Payload,
            bool HasErrorOutput = false,
            int? ExitCode = null)
        {
            this.Succeeded = Succeeded;
            this.Payload = Payload;
            this.HasErrorOutput = HasErrorOutput;
            this.ExitCode = ExitCode;

            _script = new Lazy<string>(() =>
            {
                bool isTaskFromResult = Payload.Contains("Task.FromResult");
                string script =
                    $$"""
                      using System.Threading.Tasks;

                      {{(isTaskFromResult ? "Task<int>" : "int")}} Main()
                      {
                          {{Payload}};
                          return {{(isTaskFromResult ? "Task.FromResult(" : "(")}}{{SuccessExitCode}});
                      }
                      """;
                return LinqScript.FromScript(
                    script,
                    """<Query Kind="Program" />""");
            }, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        private readonly Lazy<string> _script;

        /// <summary></summary>
        public readonly bool Succeeded;

        /// <summary></summary>
        public readonly string Payload;

        /// <summary></summary>
        public readonly bool HasErrorOutput;

        /// <summary></summary>
        public readonly int? ExitCode;

        /// <summary>
        /// Convert parameters into a record.
        /// </summary>
        /// <param name="succeeded"></param>
        /// <param name="payload"></param>
        /// <param name="hasErrorOutput"></param>
        /// <param name="exitCode"></param>
        [UsedImplicitly]
        public void Deconstruct(out bool succeeded, out string payload, out bool hasErrorOutput, out int? exitCode)
        {
            succeeded = Succeeded;
            payload = Payload;
            hasErrorOutput = HasErrorOutput;
            exitCode = ExitCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Payload;

        /// <summary>
        /// Return the generated script.
        /// </summary>
        public string Script => _script.Value;
    }
}
