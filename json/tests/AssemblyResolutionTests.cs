using System.IO;
using System.Reflection;
using Xunit;

namespace dotnet.text.json.Tests
{
    /// <summary>
    /// Guards against the test suite silently binding to the framework's built-in
    /// System.Text.Json instead of the locally built dotnet.text.json library:
    /// the serializer types must resolve to the local assembly (verified by Assembly.Location).
    /// </summary>
    public class AssemblyResolutionTests
    {
        [Fact]
        public void JsonSerializerResolvesToLocallyBuiltAssembly()
        {
            Assembly asm = typeof(JsonSerializer).Assembly;

            Assert.StartsWith("dotnet.text.json", asm.FullName);
            Assert.NotEmpty(asm.Location);

            // The framework's built-in System.Text.Json lives in the shared framework
            // directory (e.g. dotnet\shared\Microsoft.NETCore.App\...); the locally built
            // assembly is deployed next to the test assembly in the test output directory.
            string testOutputDirectory = Path.GetDirectoryName(typeof(AssemblyResolutionTests).Assembly.Location);
            Assert.True(
                asm.Location.StartsWith(testOutputDirectory, StringComparison.OrdinalIgnoreCase),
                $"JsonSerializer resolved to {asm.FullName} @ {asm.Location}, expected a local assembly in {testOutputDirectory}");
        }
    }
}
