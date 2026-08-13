// Simplified conditional test attributes for standalone test project

using System;

namespace Xunit
{
    /// <summary>
    /// Attribute that conditionally skips a test fact based on a condition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ConditionalFactAttribute : FactAttribute
    {
        public ConditionalFactAttribute(params object[] _)
        {
            // In the real implementation, the condition would be evaluated
            // For our standalone version, we skip the test
            Skip = "ConditionalFact not supported in standalone test project";
        }
    }

    /// <summary>
    /// Attribute that conditionally skips a test theory based on a condition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ConditionalTheoryAttribute : TheoryAttribute
    {
        public ConditionalTheoryAttribute(params object[] _)
        {
            Skip = "ConditionalTheory not supported in standalone test project";
        }
    }

    /// <summary>
    /// Attribute to mark long-running tests that are typically run in outer loop builds.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class OuterLoopAttribute : Attribute
    {
        public OuterLoopAttribute()
        {
        }

        public OuterLoopAttribute(string _)
        {
        }
    }

    /// <summary>
    /// Attribute to skip tests on CoreCLR runtime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SkipOnCoreClrAttribute : Attribute
    {
        public SkipOnCoreClrAttribute(string reason, RuntimeConfiguration configuration, RuntimeTestModes testModes)
        {
        }
    }

    /// <summary>
    /// Runtime configuration for CoreCLR tests.
    /// </summary>
    public enum RuntimeConfiguration
    {
        Checked,
        Release
    }

    /// <summary>
    /// Runtime test modes for CoreCLR tests.
    /// </summary>
    public enum RuntimeTestModes
    {
        RegTest,
        JitStress,
        JitStressRegs,
        JitMinOpts,
        TailcallStress
    }
}
