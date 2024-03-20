using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LPRun;
using NUnit.Framework;

namespace NY.Dataverse.LINQPadDriver.Tests;

/// <summary>
/// 
/// </summary>
[TestFixture]
public class LINQPadRunnerTests : LINQPadRunnerTestFixture
{
    /// <summary>
    ///     Validate that we can load data and run LPRun successfully.
    /// </summary>
    /// <param name="testData"></param>
    [Test]
    [TestCaseSource(nameof(ScriptFromSourceTestDataTestsData))]
    public async Task TestSourceScript(ScriptTestData testData)
    {
        Runner.Result result = await ExecuteAsync(testData.Script);

        result.Success.Should().Be(testData.Succeeded);
        result.ExitCode.Should().Be(testData.ExitCode ??
                                    (testData.Succeeded
                                        ? ScriptTestData.SuccessExitCode
                                        : ScriptTestData.ErrorExitCode));

        if (testData.HasErrorOutput)
            result.Error.Should().NotBeNullOrEmpty().And.Contain(ScriptTestData.ErrorMessage.ToString());
        else
            result.Error.Should().BeNullOrEmpty();
    }

    private static IEnumerable<ScriptTestData> ScriptFromSourceTestDataTestsData()
    {
        yield return new ScriptTestData(
            false, $"""Console.Error.WriteLine("{ScriptTestData.ErrorMessage}")""", true,
            ScriptTestData.SuccessExitCode);
        yield return new ScriptTestData(
            true, $"""Console.WriteLine("{ScriptTestData.SuccessMessage}")""");
        yield return new ScriptTestData(
            true, $"Environment.Exit({ScriptTestData.SuccessExitCode})");
        yield return new ScriptTestData(
            false, $"Environment.Exit({ScriptTestData.ErrorExitCode})");
        yield return new ScriptTestData(
            true, "//");
        yield return new ScriptTestData(
            false,
            $"""throw new Exception("{ScriptTestData.ErrorMessage}")""",
            true, 1); // LPRun exit code.
        yield return new ScriptTestData(
            false, $"return {ScriptTestData.ErrorExitCode}");
        yield return new ScriptTestData(
            true, $"return {ScriptTestData.SuccessExitCode}");
        yield return new ScriptTestData(
            false, $"return Task.FromResult({ScriptTestData.ErrorExitCode})");
        yield return new ScriptTestData(
            true, $"return Task.FromResult({ScriptTestData.SuccessExitCode})");
    }
}