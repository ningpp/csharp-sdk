// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace dotnet.xml.Xsl.XsltOld
{
    using System;
    using System.Diagnostics;
    using dotnet.xml;
    using dotnet.xml.xpath;

    internal class DocumentScope
    {
        protected NamespaceDecl scopes;

        internal NamespaceDecl Scopes
        {
            get { return this.scopes; }
        }

        internal NamespaceDecl AddNamespace(string prefix, string uri, string prevDefaultNsUri)
        {
            this.scopes = new NamespaceDecl(prefix, uri, prevDefaultNsUri, this.scopes);
            return this.scopes;
        }

        internal string ResolveAtom(string prefix)
        {
            Debug.Assert(prefix != null && prefix.Length > 0);

            for (NamespaceDecl scope = this.scopes; scope != null; scope = scope.Next)
            {
                if (Ref.Equal(scope.Prefix, prefix))
                {
                    Debug.Assert(scope.Uri != null);
                    return scope.Uri;
                }
            }

            return null;
        }

        internal string ResolveNonAtom(string prefix)
        {
            Debug.Assert(prefix != null && prefix.Length > 0);

            for (NamespaceDecl scope = this.scopes; scope != null; scope = scope.Next)
            {
                if (scope.Prefix == prefix)
                {
                    Debug.Assert(scope.Uri != null);
                    return scope.Uri;
                }
            }
            return null;
        }
    }
}
