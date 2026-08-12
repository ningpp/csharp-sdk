using System;
using System.Runtime.InteropServices;

namespace Xunit
{
    // Stub for corefx test infrastructure - SkipOnTargetFramework attribute
    // In the original build system, this skips tests on certain target frameworks
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class SkipOnTargetFrameworkAttribute : Attribute
    {
        public SkipOnTargetFrameworkAttribute(string targetFrameworkMoniker, string reason = null) { }
    }

    // Stub for TargetFrameworkMonikers constants
    internal static class TargetFrameworkMonikers
    {
        public const string NetFramework = "netframework";
        public const string Netcoreapp = "netcoreapp";
        public const string Uap = "uap";
        public const string Netstandard = "netstandard";
    }

    // PlatformSpecific attribute that skips tests on non-matching platforms
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class PlatformSpecificAttribute : Attribute
    {
        public PlatformSpecificAttribute(string testPlatform) { }
    }

    // Stub for TestPlatforms constants
    internal static class TestPlatforms
    {
        public const string Windows = "Windows";
        public const string Linux = "Linux";
        public const string macOS = "macOS";
        public const string FreeBSD = "FreeBSD";
        public const string NetBSD = "NetBSD";
        public const string AnyUnix = "AnyUnix";
    }
}
