// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System
{
    [Flags]
    public enum TargetFrameworkMonikers
    {
        NetFramework = 0x1,
        NetCoreApp = 0x2,
        NetStandard = 0x4,
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SkipOnTargetFrameworkAttribute : Attribute
    {
        public SkipOnTargetFrameworkAttribute(TargetFrameworkMonikers frameworks, string reason = null)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OuterLoopAttribute : Attribute
    {
    }
}
