// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Sdk;

namespace dotnet.text.json
{
    // Stub types to replace .NET runtime test infrastructure
    // Note: These don't inherit from FactAttribute to avoid xUnit analyzer conflicts

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OuterLoopAttribute : Attribute
    {
        public OuterLoopAttribute(string message = null) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PlatformSpecificAttribute : Attribute
    {
        public PlatformSpecificAttribute(TestPlatforms platforms) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ConditionalTheoryAttribute : Attribute
    {
        public ConditionalTheoryAttribute(string condition = null) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ConditionalFactAttribute : Attribute
    {
        public ConditionalFactAttribute(string condition = null) { }
        public ConditionalFactAttribute(string condition, string skipMessage) { }
        public ConditionalFactAttribute(Type type, string condition) { }
        public ConditionalFactAttribute(Type type, string condition, string skipMessage) { }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class ActiveIssueAttribute : BeforeAfterTestAttribute
    {
        private readonly string _issueUrl;
        private readonly TestPlatforms _platforms;
        private readonly Type? _conditionType;
        private readonly string[] _conditionMemberNames;

        public ActiveIssueAttribute(string issueUrl, TestPlatforms platforms = TestPlatforms.Any)
        {
            _issueUrl = issueUrl;
            _platforms = platforms;
        }

        public ActiveIssueAttribute(string issueUrl, Type type, params string[] memberNames)
        {
            _issueUrl = issueUrl;
            _platforms = 0;
            _conditionType = type;
            _conditionMemberNames = memberNames ?? Array.Empty<string>();
        }

        public ActiveIssueAttribute(string issueUrl, TestPlatforms platforms, Type type, params string[] memberNames)
        {
            _issueUrl = issueUrl;
            _platforms = platforms;
            _conditionType = type;
            _conditionMemberNames = memberNames ?? Array.Empty<string>();
        }

        private static TestPlatforms CurrentPlatform
        {
            get
            {
                if (OperatingSystem.IsWindows()) return TestPlatforms.Windows;
                if (OperatingSystem.IsLinux()) return TestPlatforms.Linux;
                if (OperatingSystem.IsMacOS()) return TestPlatforms.OSX;
                if (OperatingSystem.IsFreeBSD()) return TestPlatforms.FreeBSD;
                if (OperatingSystem.IsAndroid()) return TestPlatforms.Android;
                if (OperatingSystem.IsIOS()) return TestPlatforms.iOS;
                if (OperatingSystem.IsBrowser()) return TestPlatforms.Browser;
                return 0;
            }
        }

        private static bool EvaluateCondition(Type type, string memberName)
        {
            MemberInfo? member = type.GetMember(memberName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).FirstOrDefault();
            object? value = member switch
            {
                PropertyInfo p => p.GetValue(null),
                FieldInfo f => f.GetValue(null),
                _ => null
            };
            return value is true;
        }

        public override void Before(MethodInfo methodUnderTest)
        {
            // Mirrors the .NET runtime test infrastructure: tests annotated with an active
            // issue are skipped only when the specified platform/condition applies.
            // Xunit.SkipException (from Xunit.SkippableFact) is turned into a skipped
            // result when the test uses [SkippableFact]/[SkippableTheory].
            bool platformApplies = _platforms == 0
                || (_platforms & (CurrentPlatform == 0 ? TestPlatforms.Any : CurrentPlatform)) != 0;

            bool conditionApplies = true;
            if (_conditionType != null)
            {
                conditionApplies = _conditionMemberNames.Length == 0
                    || _conditionMemberNames.All(m => EvaluateCondition(_conditionType, m));
            }

            if (platformApplies && conditionApplies)
            {
                throw new Xunit.SkipException($"Active issue: {_issueUrl}");
            }
        }
    }

    public static class PlatformDetection
    {
        public static bool IsWindows => OperatingSystem.IsWindows();
        public static bool IsLinux => OperatingSystem.IsLinux();
        public static bool IsMacOS => OperatingSystem.IsMacOS();
        public static bool IsAndroid => OperatingSystem.IsAndroid();
        public static bool IsIOS => OperatingSystem.IsIOS();
        public static bool IsBrowser => OperatingSystem.IsBrowser();
        public static bool IsFreeBSD => OperatingSystem.IsFreeBSD();
        public static bool IsNetBSD => false; // OperatingSystem.IsNetBSD() is not available in all TFMs
        public static bool IsInitialized => true;
        public static bool IsMonoInterpreter => false;
        public static bool IsMonoRuntime => false;
        public static bool IsNotPlatformDetection => true;
        public static bool IsNetFramework => false;
    }

    // Stub for AssertExtensions from .NET runtime test infrastructure
    public static class AssertExtensions
    {
        public static void Throws<T>(Action testCode) where T : Exception
        {
            Assert.Throws<T>(testCode);
        }

        public static T Throws<T>(string paramName, Action testCode) where T : Exception
        {
            var ex = Assert.Throws<T>(testCode);
            string actualParamName = (ex as ArgumentException)?.ParamName ?? ex.Data["ParamName"] as string;
            Assert.Equal(paramName, actualParamName);
            return ex;
        }

        public static T Throws<T>(Action testCode, string expectedMessage) where T : Exception
        {
            var ex = Assert.Throws<T>(testCode);
            Assert.Contains(expectedMessage, ex.Message);
            return ex;
        }

        public static void ThrowsAny(Type exceptionType1, Type exceptionType2, Action testCode)
        {
            var ex = Assert.ThrowsAny<Exception>(testCode);
            if (ex.GetType() != exceptionType1 && ex.GetType() != exceptionType2)
            {
                throw new AssertionException($"Expected {exceptionType1} or {exceptionType2} but got {ex.GetType()}");
            }
        }

        public static async Task ThrowsAsync<T>(Func<Task> testCode) where T : Exception
        {
            await Assert.ThrowsAsync<T>(testCode);
        }

        public static async Task<T> ThrowsAsync<T>(string paramName, Func<Task> testCode) where T : Exception
        {
            var ex = await Assert.ThrowsAsync<T>(testCode);
            string actualParamName = (ex as ArgumentException)?.ParamName ?? ex.Data["ParamName"] as string;
            Assert.Equal(paramName, actualParamName);
            return ex;
        }

        public static async Task<T> ThrowsAsync<T>(Func<Task> testCode, string expectedMessage) where T : Exception
        {
            var ex = await Assert.ThrowsAsync<T>(testCode);
            Assert.Contains(expectedMessage, ex.Message);
            return ex;
        }

        public static T ThrowsAny<T>(Action testCode) where T : Exception
        {
            return Assert.Throws<T>(testCode);
        }

        public static void EqualWithComparison<T>(T expected, T actual)
        {
            Assert.Equal(expected, actual);
        }

        public static void DoesNotThrow(Action testCode)
        {
            testCode();
        }
    }

    [Flags]
    public enum TestPlatforms
    {
        Windows = 1,
        Linux = 2,
        OSX = 4,
        FreeBSD = 8,
        NetBSD = 16,
        Illumos = 32,
        Solaris = 64,
        iOS = 128,
        tvOS = 256,
        Android = 512,
        Browser = 1024,
        Windows8x = 2048,
        LinuxBionic = 4096,
        Any = Windows | Linux | OSX | FreeBSD | NetBSD | Illumos | Solaris | iOS | tvOS | Android | Browser | Windows8x | LinuxBionic
    }

    /// <summary>
    /// Exception thrown to skip a test (replaces Microsoft.DotNet.XUnitExtensions.SkipTestException).
    /// </summary>
    public class SkipTestException : Exception
    {
        public SkipTestException(string message) : base(message) { }
        public SkipTestException(string message, Exception inner) : base(message, inner) { }
        public SkipTestException() : base() { }
    }

    /// <summary>
    /// Assertion exception from Xunit.Sdk, provided here as a stub.
    /// </summary>
    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message) { }
        public AssertionException(string message, Exception inner) : base(message, inner) { }
        public AssertionException() : base() { }
    }

    /// <summary>
    /// Extension method to provide ParamName property on Exception for backwards compat with .NET runtime test helpers.
    /// </summary>
    public static class ExceptionExtensions
    {
        public static string? ParamName(this Exception ex)
        {
            return ex.Data["ParamName"] as string;
        }
    }
}
