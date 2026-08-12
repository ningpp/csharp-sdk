using System;
using System.Collections.Generic;

namespace dotnet.system
{
    internal static class SR
    {
        private static readonly Dictionary<string, string> s_resources = new Dictionary<string, string>
        {
            { nameof(Argument_AddingDuplicate), "An item with the same key has already been added. Key: {0}" },
            { nameof(net_uri_BadAuthority), "Invalid URI: The Authority/Host could not be parsed." },
            { nameof(net_uri_BadAuthorityTerminator), "Invalid URI: The Authority/Host cannot end with a backslash character ('\\')." },
            { nameof(net_uri_BadFormat), "Invalid URI: The format of the URI could not be determined." },
            { nameof(net_uri_NeedFreshParser), "The URI parser instance passed into 'uriParser' parameter is already registered with the scheme name '{0}'." },
            { nameof(net_uri_AlreadyRegistered), "A URI scheme name '{0}' already has a registered custom parser." },
            { nameof(net_uri_BadHostName), "Invalid URI: The hostname could not be parsed." },
            { nameof(net_uri_BadPort), "Invalid URI: Invalid port specified." },
            { nameof(net_uri_BadScheme), "Invalid URI: The URI scheme is not valid." },
            { nameof(net_uri_BadString), "Invalid URI: There is an invalid sequence in the string." },
            { nameof(net_uri_BadUserPassword), "Invalid URI: The username:password construct is badly formed." },
            { nameof(net_uri_CannotCreateRelative), "A relative URI cannot be created because the 'uriString' parameter represents an absolute URI." },
            { nameof(net_uri_SchemeLimit), "Invalid URI: The Uri scheme is too long." },
            { nameof(net_uri_EmptyUri), "Invalid URI: The URI is empty." },
            { nameof(net_uri_InvalidUriKind), "The value '{0}' passed for the UriKind parameter is invalid." },
            { nameof(net_uri_MustRootedPath), "Invalid URI: A Dos path must be rooted, for example, 'c:\\'." },
            { nameof(net_uri_NotAbsolute), "This operation is not supported for a relative URI." },
            { nameof(net_uri_PortOutOfRange), "A derived type '{0}' has reported an invalid value for the Uri port '{1}'." },
            { nameof(net_uri_SizeLimit), "Invalid URI: The Uri string is too long." },
            { nameof(net_uri_UserDrivenParsing), "A derived type '{0}' is responsible for parsing this Uri instance. The base implementation must not be used." },
            { nameof(net_uri_NotJustSerialization), "UriComponents.SerializationInfoString must not be combined with other UriComponents." },
            { nameof(net_uri_BadUnicodeHostForIdn), "An invalid Unicode character by IDN standards was specified in the host." },
            { nameof(Argument_ExtraNotValid), "Extra portion of URI not valid." },
            { nameof(Argument_InvalidUriSubcomponent), "The subcomponent, {0}, of this uri is not valid." },
            { nameof(Arg_KeyNotFoundWithKey), "The given key '{0}' was not present in the dictionary." },
        };

        public static string Argument_AddingDuplicate => GetResourceString(nameof(Argument_AddingDuplicate));
        public static string net_uri_BadAuthority => GetResourceString(nameof(net_uri_BadAuthority));
        public static string net_uri_BadAuthorityTerminator => GetResourceString(nameof(net_uri_BadAuthorityTerminator));
        public static string net_uri_BadFormat => GetResourceString(nameof(net_uri_BadFormat));
        public static string net_uri_NeedFreshParser => GetResourceString(nameof(net_uri_NeedFreshParser));
        public static string net_uri_AlreadyRegistered => GetResourceString(nameof(net_uri_AlreadyRegistered));
        public static string net_uri_BadHostName => GetResourceString(nameof(net_uri_BadHostName));
        public static string net_uri_BadPort => GetResourceString(nameof(net_uri_BadPort));
        public static string net_uri_BadScheme => GetResourceString(nameof(net_uri_BadScheme));
        public static string net_uri_BadString => GetResourceString(nameof(net_uri_BadString));
        public static string net_uri_BadUserPassword => GetResourceString(nameof(net_uri_BadUserPassword));
        public static string net_uri_CannotCreateRelative => GetResourceString(nameof(net_uri_CannotCreateRelative));
        public static string net_uri_SchemeLimit => GetResourceString(nameof(net_uri_SchemeLimit));
        public static string net_uri_EmptyUri => GetResourceString(nameof(net_uri_EmptyUri));
        public static string net_uri_InvalidUriKind => GetResourceString(nameof(net_uri_InvalidUriKind));
        public static string net_uri_MustRootedPath => GetResourceString(nameof(net_uri_MustRootedPath));
        public static string net_uri_NotAbsolute => GetResourceString(nameof(net_uri_NotAbsolute));
        public static string net_uri_PortOutOfRange => GetResourceString(nameof(net_uri_PortOutOfRange));
        public static string net_uri_SizeLimit => GetResourceString(nameof(net_uri_SizeLimit));
        public static string net_uri_UserDrivenParsing => GetResourceString(nameof(net_uri_UserDrivenParsing));
        public static string net_uri_NotJustSerialization => GetResourceString(nameof(net_uri_NotJustSerialization));
        public static string net_uri_BadUnicodeHostForIdn => GetResourceString(nameof(net_uri_BadUnicodeHostForIdn));
        public static string Argument_ExtraNotValid => GetResourceString(nameof(Argument_ExtraNotValid));
        public static string Argument_InvalidUriSubcomponent => GetResourceString(nameof(Argument_InvalidUriSubcomponent));
        public static string Arg_KeyNotFoundWithKey => GetResourceString(nameof(Arg_KeyNotFoundWithKey));

        private static string GetResourceString(string name)
        {
            return s_resources.TryGetValue(name, out string? value) ? value : name;
        }

        public static string Format(string resourceFormat, params object[] args)
        {
            if (args is null || args.Length == 0)
                return resourceFormat;

            return string.Format(resourceFormat, args);
        }
    }
}
