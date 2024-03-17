using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPRun;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.ContextMask)]

namespace NY.Dataverse.LINQPadDriver.Tests;

/// <summary>
///
/// </summary>
[TestFixture]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed partial class LPRunTests
{
    private const int RetryCount = 3;

    private static readonly TimeSpan RetryTimeout = TimeSpan.FromSeconds(10);

    private static readonly TimeSpan WaitTimeout = TimeSpan.FromMinutes(3);

    private sealed record FileEncoding(string FileName, Encoding Encoding);

    /// <summary>
    /// Initialize the driver and ensure it is not installed with NuGet.
    /// </summary>
    [OneTimeSetUp]
    public void Init()
    {
        const string driverFileName = "NY.Dataverse.LINQPadDriver";

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Driver.EnsureNotInstalledViaNuGet(driverFileName);
        Driver.InstallWithDepsJson(driverFileName, $"{driverFileName}.dll", "src");

        CreateFileEncodings(@"Encoding\Utf8Cp65001", Encoding.Default);
        CreateFileEncodings(@"Encoding\German\Cp1252", Encoding.GetEncoding("Windows-1252"));
        
        return;

        static void CreateFileEncodings(string baseFile, Encoding encoding)
        {
            var directory = Path.GetDirectoryName(baseFile);
            var content = File.ReadAllText(GetFilePath(baseFile), encoding);

            Array.ForEach(GetFileEncodings().ToArray(), WriteFiles);
            
            return;

            static IEnumerable<FileEncoding> GetFileEncodings()
            {
                yield return new FileEncoding("Utf16BomCp1200", Encoding.Unicode);
                yield return new FileEncoding("Utf16BomCp1201", Encoding.BigEndianUnicode);
                yield return new FileEncoding("Utf8BomCp65001", Encoding.UTF8);
                yield return new FileEncoding("Utf32Bom", Encoding.UTF32);
            }

            void WriteFiles(FileEncoding fileEncoding)
            {
                File.WriteAllText(GetFilePath(fileEncoding.FileName), content, fileEncoding.Encoding);
            }

            string GetFilePath(string fileName)
            {
                return Context.GetDataFullPath(Path.Combine(directory!, $"{Path.GetFileName(fileName)}.csv"));
            }
        }
    }

    private static Task<Runner.Result> ExecuteAsync(string scriptFile) =>
        Runner.ExecuteAsync(scriptFile,
            WaitTimeout
            , new Runner.RetryOnError(RetryCount, RetryTimeout)
        );
}