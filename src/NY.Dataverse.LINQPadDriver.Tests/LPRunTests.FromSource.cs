using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using LPRun;
using NUnit.Framework;

namespace NY.Dataverse.LINQPadDriver.Tests;

[TestFixture]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial class LPRunTests
{
    private const int SuccessExitCode = 0;
    private static readonly int ErrorExitCode = new Random().Next() % 250 + 2;

    private static readonly string SuccessMessage = Guid.NewGuid().ToString();
    private static readonly string ErrorMessage = Guid.NewGuid().ToString();

    /// <summary>
    /// </summary>
    /// <param name="Succeeded"></param>
    /// <param name="Payload"></param>
    /// <param name="HasErrorOutput"></param>
    /// <param name="ExitCode"></param>
    public sealed record ScriptFromSourceTestData(
        bool Succeeded,
        string Payload,
        bool HasErrorOutput = false,
        int? ExitCode = null);

    /// <summary>
    ///     Validate that we can load data and run LPRun successfully.
    /// </summary>
    /// <param name="testData"></param>
    [Test]
    [TestCaseSource(nameof(ScriptFromSourceTestDataTestsData))]
    public async Task Execute_ScriptFromSource_Success(ScriptFromSourceTestData testData)
    {
        var scriptFile = LinqScript.FromScript(GetScript(), """<Query Kind="Program" />""");

        var result = await ExecuteAsync(scriptFile);

        result.Success.Should().Be(testData.Succeeded);
        result.ExitCode.Should().Be(testData.ExitCode ?? (testData.Succeeded ? SuccessExitCode : ErrorExitCode));

        if (testData.HasErrorOutput)
            result.Error.Should().NotBeNullOrEmpty().And.Contain(ErrorMessage);
        else
            result.Error.Should().BeNullOrEmpty();

        return;

        string GetScript()
        {
            var isTaskFromResult = testData.Payload.Contains("Task.FromResult");
            return
                $$"""
                  using System.Threading.Tasks;

                  {{(isTaskFromResult ? "Task<int>" : "int")}} Main()
                  {
                      {{testData.Payload}};
                      return {{(isTaskFromResult ? "Task.FromResult(" : "(")}}{{SuccessExitCode}});
                  }
                  """;
        }
    }

    private static IEnumerable<ScriptFromSourceTestData> ScriptFromSourceTestDataTestsData()
    {
        yield return new ScriptFromSourceTestData(
            false, $"""Console.Error.WriteLine("{ErrorMessage}")""", true, SuccessExitCode);
        yield return new ScriptFromSourceTestData(
            true, $"""Console.WriteLine("{SuccessMessage}")""");
        yield return new ScriptFromSourceTestData(
            true, $"Environment.Exit({SuccessExitCode})");
        yield return new ScriptFromSourceTestData(
            false, $"Environment.Exit({ErrorExitCode})");
        yield return new ScriptFromSourceTestData(
            true, "//");
        yield return new ScriptFromSourceTestData(
            false, 
            $"""throw new Exception("{ErrorMessage}")""", 
            true, 1); // LPRun exit code.
        yield return new ScriptFromSourceTestData(
            false, $"return {ErrorExitCode}");
        yield return new ScriptFromSourceTestData(
            true, $"return {SuccessExitCode}");
        yield return new ScriptFromSourceTestData(
            false, $"return Task.FromResult({ErrorExitCode})");
        yield return new ScriptFromSourceTestData(
            true, $"return Task.FromResult({SuccessExitCode})");
    }
}