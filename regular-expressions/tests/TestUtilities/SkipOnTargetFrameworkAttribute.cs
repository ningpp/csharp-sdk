// Simplified SkipOnTargetFramework attribute for standalone test project

using System;

namespace Xunit
{
    /// <summary>
    /// Used to skip tests based on target framework.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class SkipOnTargetFrameworkAttribute : Attribute
    {
        public TargetFrameworkMonikers Framework { get; }
        public string SkipReason { get; }

        public SkipOnTargetFrameworkAttribute(TargetFrameworkMonikers framework)
        {
            Framework = framework;
            SkipReason = GetSkipReason(framework);
        }

        public SkipOnTargetFrameworkAttribute(TargetFrameworkMonikers framework, string reason)
        {
            Framework = framework;
            SkipReason = reason;
        }

        private static string GetSkipReason(TargetFrameworkMonikers framework)
        {
            bool isNetCore = Environment.Version.Major >= 5 ||
                System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.StartsWith(".NET Core", StringComparison.OrdinalIgnoreCase);

            bool isNetFramework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);

            if (framework == TargetFrameworkMonikers.NetCoreApp && !isNetCore)
                return "Test only runs on .NET Core";

            if (framework == TargetFrameworkMonikers.NetFramework && !isNetFramework)
                return "Test only runs on .NET Framework";

            if ((framework & TargetFrameworkMonikers.NetFramework) == 0 && isNetFramework)
                return "Test does not run on .NET Framework";

            if ((framework & TargetFrameworkMonikers.NetCoreApp) == 0 && isNetCore)
                return "Test does not run on .NET Core";

            return "Test skipped on this framework";
        }
    }

    [Flags]
    public enum TargetFrameworkMonikers
    {
        NetFramework = 1,
        NetCoreApp = 2,
    }
}
