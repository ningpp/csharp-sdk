// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace dotnet.text.regularexpressions
{
    internal static class SR
    {
        private static readonly Dictionary<string, string> s_strings = new Dictionary<string, string>
        {
            { "Generic", "Regular expression parser error '{0}' at offset {1}." },
            { "AlternationHasNamedCapture", "Alternation conditions do not capture and cannot be named." },
            { "AlternationHasComment", "Alternation conditions cannot be comments." },
            { "Arg_ArrayPlusOffTooSmall", "Destination array is not long enough to copy all the items in the collection. Check array index and length." },
            { "ArgumentNull_ArrayWithNullElements", "The array cannot contain null elements." },
            { "ShorthandClassInCharacterRange", "Cannot include class \\{0} in character range." },
            { "ShorthandClassInCharacterRangeNoPlaceholder", "Cannot include class in character range." },
            { "BeginIndexNotNegative", "Start index cannot be less than 0 or greater than input length." },
            { "QuantifierOrCaptureGroupOutOfRange", "Capture group numbers must be less than or equal to Int32.MaxValue." },
            { "CaptureGroupOfZero", "Capture number cannot be zero." },
            { "CountTooSmall", "Count cannot be less than -1." },
            { "EnumNotStarted", "Enumeration has either not started or has already finished." },
            { "AlternationHasMalformedCondition", "Illegal conditional (?(...)) expression." },
            { "IllegalDefaultRegexMatchTimeoutInAppDomain", "AppDomain data '{0}' contains the invalid value or object '{1}' for specifying a default matching timeout for System.Text.RegularExpressions.Regex." },
            { "UnescapedEndingBackslash", "Illegal \\ at end of pattern." },
            { "ReversedQuantifierRange", "Illegal {x,y} with x > y." },
            { "InvalidUnicodePropertyEscape", "Incomplete \\p{X} character escape." },
            { "InternalError_ScanRegex", "Internal error in ScanRegex." },
            { "CaptureGroupNameInvalid", "Invalid group name: Group names must begin with a word character." },
            { "InvalidEmptyArgument", "Argument {0} cannot be zero-length." },
            { "LengthNotNegative", "Length cannot be less than 0 or exceed input length." },
            { "MalformedNamedReference", "Malformed \\k<...> named back reference." },
            { "AlternationHasMalformedReference", "(?({0}) ) malformed." },
            { "AlternationHasMalformedReferenceNoPlaceholder", "Alternation has malformed reference." },
            { "MalformedUnicodePropertyEscape", "Malformed \\p{X} character escape." },
            { "MakeException", "Invalid pattern '{0}' at offset {1}. {2}" },
            { "MissingControlCharacter", "Missing control character." },
            { "NestedQuantifiersNotParenthesized", "Nested quantifier '{0}'." },
            { "NestedQuantifiersNotParenthesizedNoPlaceholder", "Nested quantifier no parenthesized." },
            { "NoResultOnFailed", "Result cannot be called on a failed Match." },
            { "InsufficientClosingParentheses", "Not enough )'s." },
            { "NotSupported_ReadOnlyCollection", "Collection is read-only." },
            { "OnlyAllowedOnce", "This operation is only allowed once per object." },
            { "PlatformNotSupported_CompileToAssembly", "This platform does not support writing compiled regular expressions to an assembly." },
            { "QuantifierAfterNothing", "Quantifier {x,y} following nothing." },
            { "RegexMatchTimeoutException_Occurred", "The RegEx engine has timed out while trying to match a pattern to an input string. This can occur for many reasons, including very large inputs or excessive backtracking caused by nested quantifiers, back-references and other factors." },
            { "ReplacementError", "Replacement pattern error." },
            { "ReversedCharacterRange", "[x-y] range in reverse order." },
            { "ExclusionGroupNotLast", "A subtraction must be the last element in a character class." },
            { "InsufficientOrInvalidHexDigits", "Insufficient hexadecimal digits." },
            { "AlternationHasTooManyConditions", "Too many | in (?()|)." },
            { "InsufficientOpeningParentheses", "Too many )'s." },
            { "UndefinedNumberedReference", "Reference to undefined group number {0}." },
            { "UndefinedNumberedReferenceNoPlaceholder", "Reference to undefined group number." },
            { "UndefinedNamedReference", "Reference to undefined group name '{0}'." },
            { "UndefinedNamedReferenceNoPlaceholder", "Reference to undefined group name." },
            { "AlternationHasUndefinedReference", "(?({0}) ) reference to undefined group." },
            { "AlternationHasUndefinedReferenceNoPlaceholder", "Alternation has a reference to undefined group." },
            { "UnexpectedOpcode", "Unexpected opcode in regular expression generation: {0}." },
            { "UnrecognizedUnicodeProperty", "Unknown property '{0}'." },
            { "UnrecognizedUnicodePropertyNoPlaceholder", "Unknown property Unicode property." },
            { "UnrecognizedControlCharacter", "Unrecognized control character." },
            { "UnrecognizedEscape", "Unrecognized escape sequence \\{0}." },
            { "InvalidGroupingConstruct", "Unrecognized grouping construct." },
            { "UnterminatedBracket", "Unterminated [] set." },
            { "UnterminatedComment", "Unterminated (?#...) comment." },
        };

        internal static string GetString(string name)
        {
            return s_strings.TryGetValue(name, out string value) ? value : name;
        }

        internal static string Format(string format, params object[] args)
        {
            string message = GetString(format);
            return string.Format(message, args);
        }

        // Strongly-typed properties for commonly used strings
        internal static string Generic => GetString("Generic");
        internal static string AlternationHasNamedCapture => GetString("AlternationHasNamedCapture");
        internal static string AlternationHasComment => GetString("AlternationHasComment");
        internal static string Arg_ArrayPlusOffTooSmall => GetString("Arg_ArrayPlusOffTooSmall");
        internal static string ArgumentNull_ArrayWithNullElements => GetString("ArgumentNull_ArrayWithNullElements");
        internal static string ShorthandClassInCharacterRange => GetString("ShorthandClassInCharacterRange");
        internal static string ShorthandClassInCharacterRangeNoPlaceholder => GetString("ShorthandClassInCharacterRangeNoPlaceholder");
        internal static string BeginIndexNotNegative => GetString("BeginIndexNotNegative");
        internal static string QuantifierOrCaptureGroupOutOfRange => GetString("QuantifierOrCaptureGroupOutOfRange");
        internal static string CaptureGroupOfZero => GetString("CaptureGroupOfZero");
        internal static string CountTooSmall => GetString("CountTooSmall");
        internal static string EnumNotStarted => GetString("EnumNotStarted");
        internal static string AlternationHasMalformedCondition => GetString("AlternationHasMalformedCondition");
        internal static string IllegalDefaultRegexMatchTimeoutInAppDomain => GetString("IllegalDefaultRegexMatchTimeoutInAppDomain");
        internal static string UnescapedEndingBackslash => GetString("UnescapedEndingBackslash");
        internal static string ReversedQuantifierRange => GetString("ReversedQuantifierRange");
        internal static string InvalidUnicodePropertyEscape => GetString("InvalidUnicodePropertyEscape");
        internal static string InternalError_ScanRegex => GetString("InternalError_ScanRegex");
        internal static string CaptureGroupNameInvalid => GetString("CaptureGroupNameInvalid");
        internal static string InvalidEmptyArgument => GetString("InvalidEmptyArgument");
        internal static string LengthNotNegative => GetString("LengthNotNegative");
        internal static string MalformedNamedReference => GetString("MalformedNamedReference");
        internal static string AlternationHasMalformedReference => GetString("AlternationHasMalformedReference");
        internal static string AlternationHasMalformedReferenceNoPlaceholder => GetString("AlternationHasMalformedReferenceNoPlaceholder");
        internal static string MalformedUnicodePropertyEscape => GetString("MalformedUnicodePropertyEscape");
        internal static string MakeException => GetString("MakeException");
        internal static string MissingControlCharacter => GetString("MissingControlCharacter");
        internal static string NestedQuantifiersNotParenthesized => GetString("NestedQuantifiersNotParenthesized");
        internal static string NestedQuantifiersNotParenthesizedNoPlaceholder => GetString("NestedQuantifiersNotParenthesizedNoPlaceholder");
        internal static string NoResultOnFailed => GetString("NoResultOnFailed");
        internal static string InsufficientClosingParentheses => GetString("InsufficientClosingParentheses");
        internal static string NotSupported_ReadOnlyCollection => GetString("NotSupported_ReadOnlyCollection");
        internal static string OnlyAllowedOnce => GetString("OnlyAllowedOnce");
        internal static string PlatformNotSupported_CompileToAssembly => GetString("PlatformNotSupported_CompileToAssembly");
        internal static string QuantifierAfterNothing => GetString("QuantifierAfterNothing");
        internal static string RegexMatchTimeoutException_Occurred => GetString("RegexMatchTimeoutException_Occurred");
        internal static string ReplacementError => GetString("ReplacementError");
        internal static string ReversedCharacterRange => GetString("ReversedCharacterRange");
        internal static string ExclusionGroupNotLast => GetString("ExclusionGroupNotLast");
        internal static string InsufficientOrInvalidHexDigits => GetString("InsufficientOrInvalidHexDigits");
        internal static string AlternationHasTooManyConditions => GetString("AlternationHasTooManyConditions");
        internal static string InsufficientOpeningParentheses => GetString("InsufficientOpeningParentheses");
        internal static string UndefinedNumberedReference => GetString("UndefinedNumberedReference");
        internal static string UndefinedNumberedReferenceNoPlaceholder => GetString("UndefinedNumberedReferenceNoPlaceholder");
        internal static string UndefinedNamedReference => GetString("UndefinedNamedReference");
        internal static string UndefinedNamedReferenceNoPlaceholder => GetString("UndefinedNamedReferenceNoPlaceholder");
        internal static string AlternationHasUndefinedReference => GetString("AlternationHasUndefinedReference");
        internal static string AlternationHasUndefinedReferenceNoPlaceholder => GetString("AlternationHasUndefinedReferenceNoPlaceholder");
        internal static string UnexpectedOpcode => GetString("UnexpectedOpcode");
        internal static string UnrecognizedUnicodeProperty => GetString("UnrecognizedUnicodeProperty");
        internal static string UnrecognizedUnicodePropertyNoPlaceholder => GetString("UnrecognizedUnicodePropertyNoPlaceholder");
        internal static string UnrecognizedControlCharacter => GetString("UnrecognizedControlCharacter");
        internal static string UnrecognizedEscape => GetString("UnrecognizedEscape");
        internal static string InvalidGroupingConstruct => GetString("InvalidGroupingConstruct");
        internal static string UnterminatedBracket => GetString("UnterminatedBracket");
        internal static string UnterminatedComment => GetString("UnterminatedComment");
    }
}
