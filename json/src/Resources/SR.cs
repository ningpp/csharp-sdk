// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Resources;

namespace dotnet.text.json
{
    public static class SR
    {
        private static readonly ResourceManager s_resourceManager = new ResourceManager("dotnet.text.json.Resources.Strings", typeof(SR).Assembly);

        internal static string GetString(string name)
        {
            return s_resourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? $"{{{name}}}";
        }

        internal static string Format(string format, params object?[] args)
        {
            // The format string is the actual format template (from SR property value).
            try
            {
                return string.Format(CultureInfo.InvariantCulture, format, args);
            }
            catch (System.FormatException)
            {
                return format;
            }
        }

        /// <summary>
        /// Gets a resource string by name. Used by tests.
        /// Returns the name itself if not found.
        /// </summary>
        public static string GetResourceString(string name)
        {
            return GetString(name);
        }

        // All resource properties - auto-generated from Strings.resx
        public static string ArrayDepthTooLarge => "The maximum configured depth of {0} has been exceeded. Cannot read next JSON array.";
        public static string ArrayModifiedDuringIteration => "The JSON array was modified during iteration.";
        public static string BufferMaximumSizeExceeded => "Cannot allocate a buffer of size {0}.";
        public static string BufferWriterAdvancedTooFar => "Cannot advance past the end of the buffer, which has a size of {0}.";
        public static string CallFlushToAvoidDataLoss => "The JSON writer needs to be flushed before getting the current state. There are {0} bytes that have not been committed to the output.";
        public static string CannotDecodeInvalidBase64 => "Cannot decode JSON text that is not encoded as valid Base64 to bytes.";
        public static string CannotEncodeInvalidUTF16 => "Cannot encode invalid UTF-16 text as JSON. Invalid surrogate value: '{0}'.";
        public static string CannotEncodeInvalidUTF8 => "Cannot encode invalid UTF-8 text as JSON. Invalid input: '{0}'.";
        public static string CannotPopulateCollection => "The collection type '{0}' is abstract, an interface, or is read only, and could not be instantiated and populated.";
        public static string CannotReadIncompleteUTF16 => "Cannot read incomplete UTF-16 JSON text as string with missing low surrogate.";
        public static string CannotReadInvalidUTF16 => "Cannot read invalid UTF-16 JSON text as string. Invalid surrogate value: '{0}'.";
        public static string CannotSerializeInvalidMember => "The type '{0}' of property '{1}' on type '{2}' is invalid for serialization or deserialization because it is a pointer type, is a ref struct, or contains generic parameters that have not been replaced by specific types.";
        public static string CannotSerializeInvalidType => "The type '{0}' is invalid for serialization or deserialization because it is a pointer type, is a ref struct, or contains generic parameters that have not been replaced by specific types.";
        public static string CannotSkip => "Cannot skip tokens on partial JSON. Either get the whole payload and create a Utf8JsonReader instance where isFinalBlock is true or call TrySkip.";
        public static string CannotStartObjectArrayAfterPrimitiveOrClose => "Cannot write the start of an object/array after a single JSON value or outside of an existing closed object/array. Current token type is '{0}'.";
        public static string CannotStartObjectArrayWithoutProperty => "Cannot write the start of an object or array without a property name. Current token type is '{0}'.";
        public static string CannotTranscodeInvalidUtf16 => "Cannot transcode invalid UTF-16 string to UTF-8 JSON text.";
        public static string CannotTranscodeInvalidUtf8 => "Cannot transcode invalid UTF-8 JSON text to UTF-16 string.";
        public static string CannotWriteCommentWithEmbeddedDelimiter => "Cannot write a comment value which contains the end of comment delimiter.";
        public static string CannotWriteEndAfterProperty => "'{0}' is invalid following a property name.";
        public static string CannotWritePropertyAfterProperty => "Cannot write a JSON property name following another property name. A JSON value is missing.";
        public static string CannotWritePropertyWithinArray => "Cannot write a JSON property within an array or as the first JSON token. Current token type is '{0}'.";
        public static string CannotWriteValueAfterPrimitiveOrClose => "Cannot write a JSON value after a single JSON value or outside of an existing closed object/array. Current token type is '{0}'.";
        public static string CannotWriteValueWithinObject => "Cannot write a JSON value within an object without a property name. Current token type is '{0}'.";
        public static string CommentHandlingMustBeValid => "The JsonCommentHandling enum must be set to one of the supported values.";
        public static string ConstructorMaxOf64Parameters => "The constructor '{0}' on type '{1}' may not have more than 64 parameters for deserialization.";
        public static string ConstructorParamIncompleteBinding => "Each parameter in constructor '{0}' on type '{1}' must bind to an object property or field on deserialization. Each parameter name must match with a property or field on the object. The match can be case-insensitive.";
        public static string ConverterCanConvertNullableRedundant => "The converter '{0}' handles type '{1}' but is being asked to convert type '{2}'. Either create a separate converter for type '{2}' or change the converter's 'CanConvert' method to only return 'true' for a single type.";
        public static string DefaultIgnoreConditionAlreadySpecified => "'IgnoreNullValues' and 'DefaultIgnoreCondition' cannot both be set to non-default values.";
        public static string DefaultIgnoreConditionInvalid => "The value cannot be 'JsonIgnoreCondition.Always'.";
        public static string DepthTooLarge => "CurrentDepth ({0}) is equal to or larger than the maximum allowed depth of {1}. Cannot write the next JSON object or array.";
        public static string DeserializeDuplicateKey => "An item with the same property name '{0}' has already been added.";
        public static string DeserializeNoConstructor => "Deserialization of types without a parameterless constructor, a singular parameterized constructor, or a parameterized constructor annotated with '{0}' is not supported. Type '{1}'.";
        public static string DeserializePolymorphicInterface => "Deserialization of interface types is not supported. Type '{0}'.";
        public static string DeserializeUnableToAssignNull => "Unable to assign 'null' to the property or field of type '{0}'.";
        public static string DeserializeUnableToAssignValue => "Unable to cast object of type '{0}' to type '{1}'.";
        public static string DeserializeUnableToConvertValue => "The JSON value could not be converted to {0}.";
        public static string DeserializeWrongType => "The specified type {0} must derive from the specific value's type {1}.";
        public static string DictionaryKeyTypeNotSupported => "The type '{0}' is not a supported Dictionary key type.";
        public static string EmptyJsonIsInvalid => "Writing an empty JSON payload (excluding comments) is invalid.";
        public static string EmptyStringToInitializeNumber => "Expected a number, but instead got empty string.";
        public static string EndOfCommentNotFound => "Expected end of comment, but instead reached end of data.";
        public static string EndOfStringNotFound => "Expected end of string, but instead reached end of data.";
        public static string ExpectedEndAfterSingleJson => "'{0}' is invalid after a single JSON value. Expected end of data.";
        public static string ExpectedEndOfDigitNotFound => "'{0}' is an invalid end of a number. Expected a delimiter.";
        public static string ExpectedFalse => "'{0}' is an invalid JSON literal. Expected the literal 'false'.";
        public static string ExpectedJsonTokens => "The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true.";
        public static string ExpectedNextDigitEValueNotFound => "'{0}' is an invalid end of a number. Expected 'E' or 'e'.";
        public static string ExpectedNull => "'{0}' is an invalid JSON literal. Expected the literal 'null'.";
        public static string ExpectedOneCompleteToken => "The input does not contain any complete JSON tokens. Expected the input to have at least one valid, complete, JSON token.";
        public static string ExpectedSeparatorAfterPropertyNameNotFound => "'{0}' is invalid after a property name. Expected a ':'.";
        public static string ExpectedStartOfPropertyNotFound => "'{0}' is an invalid start of a property name. Expected a '\"'.";
        public static string ExpectedStartOfPropertyOrValueAfterComment => "'{0}' is an invalid start of a property name or value, after a comment.";
        public static string ExpectedStartOfPropertyOrValueNotFound => "Expected start of a property name or value, but instead reached end of data.";
        public static string ExpectedStartOfValueNotFound => "'{0}' is an invalid start of a value.";
        public static string ExpectedTrue => "'{0}' is an invalid JSON literal. Expected the literal 'true'.";
        public static string ExpectedValueAfterPropertyNameNotFound => "Expected a value, but instead reached end of data.";
        public static string ExtensionDataCannotBindToCtorParam => "The extension data property '{0}' on type '{1}' cannot bind with a parameter in constructor '{2}'.";
        public static string FailedToGetLargerSpan => "The 'IBufferWriter' could not provide an output buffer that is large enough to continue writing.";
        public static string FormatBoolean => "The JSON value is not in a supported Boolean format.";
        public static string FormatByte => "Either the JSON value is not in a supported format, or is out of bounds for an unsigned byte.";
        public static string FormatDateTime => "The JSON value is not in a supported DateTime format.";
        public static string FormatDateTimeOffset => "The JSON value is not in a supported DateTimeOffset format.";
        public static string FormatDecimal => "The JSON value is either too large or too small for a Decimal.";
        public static string FormatDouble => "Either the JSON value is not in a supported format, or is out of bounds for a Double.";
        public static string FormatGuid => "The JSON value is not in a supported Guid format.";
        public static string FormatInt16 => "Either the JSON value is not in a supported format, or is out of bounds for an Int16.";
        public static string FormatInt32 => "Either the JSON value is not in a supported format, or is out of bounds for an Int32.";
        public static string FormatInt64 => "Either the JSON value is not in a supported format, or is out of bounds for an Int64.";
        public static string FormatSByte => "Either the JSON value is not in a supported format, or is out of bounds for a signed byte.";
        public static string FormatSingle => "Either the JSON value is not in a supported format, or is out of bounds for a Single.";
        public static string FormatUInt16 => "Either the JSON value is not in a supported format, or is out of bounds for a UInt16.";
        public static string FormatUInt32 => "Either the JSON value is not in a supported format, or is out of bounds for a UInt32.";
        public static string FormatUInt64 => "Either the JSON value is not in a supported format, or is out of bounds for a UInt64.";
        public static string FoundInvalidCharacter => "'{0}' is invalid after a value. Expected either ',', '}}', or ']'.";
        public static string IgnoreConditionOnValueTypeInvalid => "The ignore condition 'JsonIgnoreCondition.WhenWritingNull' is not valid on value-type member '{0}' on type '{1}'. Consider using 'JsonIgnoreCondition.WhenWritingDefault'.";
        public static string InvalidCast => "Cannot get the value of a token type '{0}' as a {1}.";
        public static string InvalidCharacterAfterEscapeWithinString => "'{0}' is an invalid escapable character within a JSON string. The string should be correctly escaped.";
        public static string InvalidCharacterAtStartOfComment => "'{0}' is invalid after '/' at the beginning of the comment. Expected either '/' or '*'.";
        public static string InvalidCharacterWithinString => "'{0}' is invalid within a JSON string. The string should be correctly escaped.";
        public static string InvalidComparison => "Cannot compare the value of a token type '{0}' to text.";
        public static string InvalidDuplicatePropertyNameHandling => "The DuplicatePropertyNameHandling enum must be set to one of the supported values.";
        public static string InvalidEndOfJsonNonPrimitive => "'{0}' is an invalid token type for the end of the JSON payload. Expected either 'EndArray' or 'EndObject'.";
        public static string InvalidHexCharacterWithinString => "'{0}' is not a hex digit following '\\u' within a JSON string. The string should be correctly escaped.";
        public static string InvalidLeadingZeroInNumber => "Invalid leading zero before '{0}'.";
        public static string JsonDocumentDoesNotSupportComments => "Comments cannot be stored in a JsonDocument, only the Skip and Disallow comment handling modes are supported.";
        public static string JsonElementHasWrongType => "The requested operation requires an element of type '{0}', but the target element has type '{1}'.";
        public static string JsonIncludeOnNonPublicInvalid => "The non-public property '{0}' on type '{1}' is annotated with 'JsonIncludeAttribute' which is invalid.";
        public static string JsonObjectDuplicateKey => "Property with name '{0}' already exists.";
        public static string JsonSerializerDoesNotSupportComments => "Comments cannot be stored when deserializing objects, only the Skip and Disallow comment handling modes are supported.";
        public static string MaxDepthMustBePositive => "Max depth must be positive.";
        public static string MetadataCannotParsePreservedObjectToImmutable => "Cannot parse a JSON object containing metadata properties like '$id' into an array or immutable collection type. Type '{0}'.";
        public static string MetadataDuplicateIdFound => "The value of the '$id' metadata property '{0}' conflicts with an existing identifier.";
        public static string MetadataIdIsNotFirstProperty => "The metadata property '$id' must be the first property in the JSON object.";
        public static string MetadataInvalidPropertyWithLeadingDollarSign => "Properties that start with '$' are not allowed on preserve mode, either escape the character or turn off preserve references by setting ReferenceHandler to null.";
        public static string MetadataInvalidReferenceToValueType => "Invalid reference to value type '{0}'.";
        public static string MetadataInvalidTokenAfterValues => "The '$values' metadata property must be a JSON array. Current token type is '{0}'.";
        public static string MetadataPreservedArrayFailed => "Deserialization failed for one of these reasons:\n1. {0}\n2. {1}";
        public static string MetadataPreservedArrayInvalidProperty => "Invalid property '{0}' found within a JSON object that must only contain metadata properties and the nested JSON array to be preserved.";
        public static string MetadataPreservedArrayPropertyNotFound => "One or more metadata properties, such as '$id' and '$values', were not found within a JSON object that must only contain metadata properties and the nested JSON array to be preserved.";
        public static string MetadataReferenceCannotContainOtherProperties => "A JSON object that contains a '$ref' metadata property must not contain any other properties.";
        public static string MetadataReferenceNotFound => "Reference '{0}' not found.";
        public static string MetadataReferenceOfTypeCannotBeAssignedToType => "The object with reference id '{0}' of type '{1}' cannot be assigned to the type '{2}'.";
        public static string MetadataValueWasNotString => "The '$id' and '$ref' metadata properties must be JSON strings. Current token type is '{0}'.";
        public static string MismatchedObjectArray => "'{0}' is invalid without a matching open.";
        public static string MultipleMembersBindWithConstructorParameter => "Members '{0}' and '{1}' on type '{2}' cannot both bind with parameter '{3}' in constructor '{4}' on deserialization.";
        public static string NamingPolicyReturnNull => "The naming policy '{0}' cannot return null.";
        public static string NotEnoughData => "There is not enough data to read through the entire JSON array or object.";
        public static string NotNodeJsonElementParent => "This JsonElement instance was not built from a JsonNode and is immutable.";
        public static string NumberHandlingConverterMustBeBuiltIn => "'JsonNumberHandlingAttribute' cannot be placed on a property, field, or type that is handled by a custom converter. See usage(s) of converter '{0}' on type '{1}'.";
        public static string NumberHandlingOnPropertyTypeMustBeNumberOrCollection => "When 'JsonNumberHandlingAttribute' is placed on a property or field, the property or field must be a number or a collection. See member '{0}' on type '{1}'.";
        public static string ObjectDepthTooLarge => "The maximum configured depth of {0} has been exceeded. Cannot read next JSON object.";
        public static string ObjectWithParameterizedCtorRefMetadataNotHonored => "Reference metadata is not honored when deserializing types using parameterized constructors. See type '{0}'.";
        public static string PropertyNameTooLarge => "The JSON property name of length {0} is too large and not supported.";
        public static string PropertyNotFound => "Property with name '{0}' not found.";
        public static string PropertyTypeMismatch => "Property with name '{0}' has a different type than expected.";
        public static string RequiredDigitNotFoundAfterDecimal => "'{0}' is invalid within a number, immediately after a decimal point ('.'). Expected a digit ('0'-'9').";
        public static string RequiredDigitNotFoundAfterSign => "'{0}' is invalid within a number, immediately after a sign character ('+' or '-'). Expected a digit ('0'-'9').";
        public static string RequiredDigitNotFoundEndOfData => "Expected a digit ('0'-'9'), but instead reached end of data.";
        public static string SerializationConverterNotCompatible => "The converter '{0}' is not compatible with the type '{1}'.";
        public static string SerializationConverterOnAttributeInvalid => "The converter specified on '{0}' does not derive from JsonConverter or have a public parameterless constructor.";
        public static string SerializationConverterOnAttributeNotCompatible => "The converter specified on '{0}' is not compatible with the type '{1}'.";
        public static string SerializationConverterRead => "The converter '{0}' read too much or not enough.";
        public static string SerializationConverterWrite => "The converter '{0}' wrote too much or not enough.";
        public static string SerializationDataExtensionPropertyInvalid => "The data extension property '{0}.{1}' does not match the required signature of IDictionary<string, JsonElement> or IDictionary<string, object>.";
        public static string SerializationDuplicateAttribute => "The attribute '{0}' cannot exist more than once on '{1}'.";
        public static string SerializationDuplicateTypeAttribute => "The type '{0}' cannot have more than one property that has the attribute '{1}'.";
        public static string SerializationInvalidBufferSize => "The value must be greater than zero.";
        public static string SerializationNotSupportedParentType => "The unsupported member type is located on type '{0}'.";
        public static string SerializationNotSupportedType => "The type '{0}' is not supported.";
        public static string SerializerConverterFactoryReturnsNull => "The converter '{0}' cannot return a null value.";
        public static string SerializerCycleDetected => "A possible object cycle was detected. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of {0}. Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles.";
        public static string SerializerOptionsImmutable => "Serializer options cannot be changed once serialization or deserialization has occurred.";
        public static string SerializerPropertyNameConflict => "The JSON property name for '{0}.{1}' collides with another property.";
        public static string SerializerPropertyNameNull => "The JSON property name for '{0}.{1}' cannot be null.";
        public static string SerializeTypeInstanceNotSupported => "Serialization and deserialization of 'System.Type' instances are not supported and should be avoided since they can lead to security issues.";
        public static string SerializeUnableToSerialize => "The object or value could not be serialized.";
        public static string SpecialNumberValuesNotSupported => ".NET number values such as positive and negative infinity cannot be written as valid JSON.";
        public static string StreamNotWritable => "Stream is not writable.";
        public static string TrailingCommaNotAllowedBeforeArrayEnd => "The JSON array contains a trailing comma at the end which is not supported in this mode. Change the reader options.";
        public static string TrailingCommaNotAllowedBeforeObjectEnd => "The JSON object contains a trailing comma at the end which is not supported in this mode. Change the reader options.";
        public static string UnexpectedEndOfDataWhileReadingComment => "Unexpected end of data while reading a comment.";
        public static string UnexpectedEndOfLineSeparator => "Found invalid line or paragraph separator character while reading a comment.";
        public static string ValueTooLarge => "The JSON value of length {0} is too large and not supported.";
        public static string ZeroDepthAtEnd => "Expected depth to be zero at the end of the JSON payload. There is an open JSON object or array that should be closed.";

        // Test data strings, loaded from embedded resources
        public static string BasicJson => GetString(nameof(BasicJson));
        public static string BasicJsonWithLargeNum => GetString(nameof(BasicJsonWithLargeNum));
        public static string BroadTree => GetString(nameof(BroadTree));
        public static string CustomerSearchApi107KB => GetString(nameof(CustomerSearchApi107KB));
        public static string CustomerSearchApi108KB => GetString(nameof(CustomerSearchApi108KB));
        public static string DeepTree => GetString(nameof(DeepTree));
        public static string DepsJsonSignalR => GetString(nameof(DepsJsonSignalR));
        public static string ExpectedBasicJson => GetString(nameof(ExpectedBasicJson));
        public static string ExpectedBasicJsonWithLargeNum => GetString(nameof(ExpectedBasicJsonWithLargeNum));
        public static string ExpectedCreateJson => GetString(nameof(ExpectedCreateJson));
        public static string ExpectedFullJsonSchema1 => GetString(nameof(ExpectedFullJsonSchema1));
        public static string ExpectedFullJsonSchema2 => GetString(nameof(ExpectedFullJsonSchema2));
        public static string ExpectedHeavyNestedJson => GetString(nameof(ExpectedHeavyNestedJson));
        public static string ExpectedHeavyNestedJsonWithArray => GetString(nameof(ExpectedHeavyNestedJsonWithArray));
        public static string ExpectedJsonWithSpecialNumFormat => GetString(nameof(ExpectedJsonWithSpecialNumFormat));
        public static string ExpectedJsonWithSpecialStrings => GetString(nameof(ExpectedJsonWithSpecialStrings));
        public static string ExpectedLargeJson => GetString(nameof(ExpectedLargeJson));
        public static string ExpectedProjectLockJson => GetString(nameof(ExpectedProjectLockJson));
        public static string FullJsonSchema1 => GetString(nameof(FullJsonSchema1));
        public static string FullJsonSchema2 => GetString(nameof(FullJsonSchema2));
        public static string HeavyNestedJson => GetString(nameof(HeavyNestedJson));
        public static string HeavyNestedJsonWithArray => GetString(nameof(HeavyNestedJsonWithArray));
        public static string HelloWorld => GetString(nameof(HelloWorld));
        public static string Json400B => GetString(nameof(Json400B));
        public static string Json400KB => GetString(nameof(Json400KB));
        public static string Json40KB => GetString(nameof(Json40KB));
        public static string Json4KB => GetString(nameof(Json4KB));
        public static string JsonWithSpecialNumFormat => GetString(nameof(JsonWithSpecialNumFormat));
        public static string JsonWithSpecialStrings => GetString(nameof(JsonWithSpecialStrings));
        public static string LargeJson => GetString(nameof(LargeJson));
        public static string LoremIpsum40Words => GetString(nameof(LoremIpsum40Words));
        public static string LoremIpsum40WordsBase64 => GetString(nameof(LoremIpsum40WordsBase64));
        public static string LotsOfNumbers => GetString(nameof(LotsOfNumbers));
        public static string LotsOfStrings => GetString(nameof(LotsOfStrings));
        public static string ParseJson => GetString(nameof(ParseJson));
        public static string ProjectLockJson => GetString(nameof(ProjectLockJson));
        public static string SimpleArrayJson => GetString(nameof(SimpleArrayJson));
        public static string SimpleObjectJson => GetString(nameof(SimpleObjectJson));
    }
}
