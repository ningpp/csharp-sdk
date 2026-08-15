// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using dotnet.text.json.Serialization;

namespace dotnet.text.json
{
    public static partial class JsonSerializer
    {
        internal static bool IsValidNumberHandlingValue(JsonNumberHandling handling)
        {
            return JsonHelpers.IsInRangeInclusive((int)handling, 0, 7);
        }
    }
}
