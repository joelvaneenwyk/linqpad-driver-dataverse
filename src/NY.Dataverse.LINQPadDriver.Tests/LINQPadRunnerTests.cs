using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using LPRun;
using NUnit.Framework;

namespace NY.Dataverse.LINQPadDriver.Tests;

/// <summary>
/// 
/// </summary>
[TestFixture]
public class LINQPadRunnerTests : LINQPadRunnerTestFixture
{
    private const int SuccessExitCode = 0;
    private static readonly int ErrorExitCode = new Random().Next() % 250 + 2;

    private static readonly string SuccessMessage = Guid.NewGuid().ToString();
    private static readonly string ErrorMessage = Guid.NewGuid().ToString();

    /// <summary>
    /// </summary>
    public sealed record ScriptTestData
    {
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

    /// <summary>
    ///     Validate that we can load data and run LPRun successfully.
    /// </summary>
    /// <param name="testData"></param>
    [Test]
    [TestCaseSource(nameof(ScriptFromSourceTestDataTestsData))]
    public async Task TestSourceScript(ScriptTestData testData)
    {
        var result = await ExecuteAsync(testData.Script);

        result.Success.Should().Be(testData.Succeeded);
        result.ExitCode.Should().Be(testData.ExitCode ?? (testData.Succeeded ? SuccessExitCode : ErrorExitCode));

        if (testData.HasErrorOutput)
            result.Error.Should().NotBeNullOrEmpty().And.Contain(ErrorMessage);
        else
            result.Error.Should().BeNullOrEmpty();
    }

    private static IEnumerable<ScriptTestData> ScriptFromSourceTestDataTestsData()
    {
        yield return new ScriptTestData(
            false, $"""Console.Error.WriteLine("{ErrorMessage}")""", true, SuccessExitCode);
        yield return new ScriptTestData(
            true, $"""Console.WriteLine("{SuccessMessage}")""");
        yield return new ScriptTestData(
            true, $"Environment.Exit({SuccessExitCode})");
        yield return new ScriptTestData(
            false, $"Environment.Exit({ErrorExitCode})");
        yield return new ScriptTestData(
            true, "//");
        yield return new ScriptTestData(
            false, 
            $"""throw new Exception("{ErrorMessage}")""", 
            true, 1); // LPRun exit code.
        yield return new ScriptTestData(
            false, $"return {ErrorExitCode}");
        yield return new ScriptTestData(
            true, $"return {SuccessExitCode}");
        yield return new ScriptTestData(
            false, $"return Task.FromResult({ErrorExitCode})");
        yield return new ScriptTestData(
            true, $"return Task.FromResult({SuccessExitCode})");
    }
}