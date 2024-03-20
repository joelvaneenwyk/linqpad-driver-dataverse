using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LPRun.Context;
using static LPRun.LPRunException;

namespace LPRun;

/// <summary>
/// Provides method for executing the LINQPad script.
/// </summary>
public static class Runner
{
    private static readonly string[] IgnoredErrorMessages =
    [
        "Downloading package",
        "Downloading NuGet package",
        "Restoring package"
    ];

    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan RetryTimeout = TimeSpan.FromSeconds(3);

    /// <summary>
    /// Executes LINQPad script using LPRun with optional timeout specified and LPRun command-line options.
    /// </summary>
    /// <param name="linqFile">The LINQPad script file to execute.</param>
    /// <param name="waitForExit">The LINQPad script execution timeout. 1 minute is the default.</param>
    /// <param name="retryOnError">The number of times to retry the operation on error and timeout between tries.</param>
    /// <param name="commandLineOptions">
    /// The additional <a href="https://www.linqpad.net/lprun.aspx">LPRun command-line</a>
    /// options. See <see cref="Options">options</see> class for details.
    /// </param>
    /// <returns>The LINQPad script execution <see cref="Result" />.</returns>
    /// <exception cref="LPRunException">Keeps the original exception as <see cref="P:System.Exception.InnerException" />.</exception>
    /// <seealso href="https://www.linqpad.net/lprun.aspx">LINQPad Command-Line and Scripting</seealso>
    public static Result Execute(string linqFile, TimeSpan? waitForExit = null, RetryOnError? retryOnError = null,
        params string[] commandLineOptions)
    {
        return ExecuteAsyncInternal(true, linqFile, waitForExit, retryOnError, commandLineOptions)
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Executes LINQPad script using LPRun with optional timeout specified and LPRun command-line options.
    /// </summary>
    /// <param name="linqFile">The LINQPad script file to execute.</param>
    /// <param name="waitForExit">The LINQPad script execution timeout. 1 minute is the default.</param>
    /// <param name="retryOnError">The number of times to retry the operation on error and timeout between tries.</param>
    /// <param name="commandLineOptions">
    /// The additional <a href="https://www.linqpad.net/lprun.aspx">LPRun command-line</a>
    /// options. See <see cref="Options">options</see> class for details.
    /// </param>
    /// <returns>A task that represents the asynchronous LINQPad script execution <see cref="Result" />.</returns>
    /// <exception cref="LPRunException">Keeps the original exception as <see cref="P:System.Exception.InnerException" />.</exception>
    /// <seealso href="https://www.linqpad.net/lprun.aspx">LINQPad Command-Line and Scripting</seealso>
    public static Task<Result> ExecuteAsync(
        string linqFile, TimeSpan? waitForExit = null,
        RetryOnError? retryOnError = null, params string[] commandLineOptions)
    {
        return ExecuteAsyncInternal(false, linqFile, waitForExit, retryOnError, commandLineOptions);
    }

    private static Task<Result> ExecuteAsyncInternal(
        bool asSync, string linqFile, TimeSpan? waitForExit,
        RetryOnError? retryOnError, params string[] commandLineOptions)
    {
        return RetryAsync(() => WrapAsync(ExecuteAsyncLocal));

        async Task<Result> RetryAsync(Func<Task<Result>> func)
        {
            int times = retryOnError?.Times ?? 1;
            TimeSpan timeout = retryOnError?.Timeout ?? RetryTimeout;

            while (true)
            {
                Result result = await func();

                if (result.Success || --times <= 0) return result;

                await Sleep();
            }

            async Task Sleep()
            {
                if (asSync)
                    Thread.Sleep(timeout);
                else
                    await Task.Delay(timeout).ConfigureAwait(false);
            }
        }

        async Task<Result> ExecuteAsyncLocal()
        {
            TimeSpan waitTime = waitForExit ?? DefaultTimeout;
            StringBuilder output = new();
            StringBuilder error = new();

            using Process process = new();

            process.OutputDataReceived += OutputDataReceivedHandler;
            process.ErrorDataReceived += ErrorDataReceivedHandler;
            process.StartInfo = new ProcessStartInfo(Exe, GetArguments())
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            bool completed = await WaitForExitAsync(
                process,
                TimeSpan.FromMilliseconds(waitTime.TotalMilliseconds));

            process.CancelOutputRead();
            process.CancelErrorRead();

            process.OutputDataReceived -= OutputDataReceivedHandler;
            process.ErrorDataReceived -= ErrorDataReceivedHandler;

            if (completed)
            {
                return new Result(output.ToString(), error.ToString(), process.ExitCode);
            }

            process.Kill();

            throw new TimeoutException($"LPRun timed out after {waitTime}");

            string GetArguments() =>
                $"""-fx={FrameworkInfo.Version.Major}.{FrameworkInfo.Version.Minor} "{GetFullPath(linqFile)}" {string.Join(" ", commandLineOptions)}""";

            void OutputDataReceivedHandler(object _, DataReceivedEventArgs e)
            {
                output.Append(e.Data);
            }

            void ErrorDataReceivedHandler(object _, DataReceivedEventArgs e)
            {
                if (Array.TrueForAll(IgnoredErrorMessages, message => e.Data?.StartsWith(message) == false))
                    error.Append(e.Data);
            }

#if NET5_0_OR_GREATER
            async Task<bool> WaitForExitAsync(Process exitProcess, TimeSpan waitForExitTimeSpan)
            {
                bool result = false;

                try
                {
                    if (asSync)
                    {
                        result = WaitForExit(exitProcess, waitForExitTimeSpan);
                    }
                    else
                    {
#if NET6_0_OR_GREATER
                        await exitProcess
                            .WaitForExitAsync()
                            .WaitAsync(waitForExitTimeSpan)
                            .ConfigureAwait(false);
#else
                        using var cancellationTokenSource = new CancellationTokenSource(waitForExitTimeSpan);
                        await exitProcess
                            .WaitForExitAsync(cancellationTokenSource.Token)
                            .ConfigureAwait(false);
#endif

                        result = true;
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ignore
                }

                return result;
            }
#else
            Task<bool> WaitForExitAsync()
            {
                return Task.FromResult(WaitForExit());
            }
#endif

            static bool WaitForExit(Process exitProcess, TimeSpan exitWaitTime)
            {
                return exitProcess.WaitForExit((int)exitWaitTime.TotalMilliseconds);
            }
        }
    }

    /// <summary>
    /// The LINQPad script execution result.
    /// </summary>
    /// <param name="Output">The LINQPad script execution captured output stream.</param>
    /// <param name="Error">The LINQPad script execution captured error stream.</param>
    /// <param name="ExitCode">The LINQPad script execution exit code.</param>
    public sealed record Result(string Output, string Error, int ExitCode)
    {
        /// <summary>
        /// Indicates that operation completed successfully.
        /// </summary>
        public bool Success => string.IsNullOrWhiteSpace(Error) && ExitCode == 0;
    }

    /// <summary>
    /// The number of times to retry the operation on error and timeout between tries.
    /// </summary>
    /// <param name="Times">The number of times to retry the operation.</param>
    /// <param name="Timeout">The timeout between tries.</param>
    public sealed record RetryOnError(int? Times = null, TimeSpan? Timeout = null);
}