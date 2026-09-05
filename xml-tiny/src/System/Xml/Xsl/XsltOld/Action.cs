// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace dotnet.xml.Xsl.XsltOld
{
    using System;
    using dotnet.xml;
    using dotnet.xml.xpath;
    using MS.Internal.Xml.XPath;
    using dotnet.xml.Xsl.XsltOld.Debugger;

    internal abstract class Action
    {
        internal const int Initialized = 0;
        internal const int Finished = -1;

        internal abstract void Execute(Processor processor, ActionFrame frame);

        internal virtual void ReplaceNamespaceAlias(Compiler compiler) { }

        // -------------- Debugger related stuff ---------
        // We have to pass ActionFrame to GetNavigator and GetVariables
        // because CopyCodeAction can't implement them without frame.count

        internal virtual DbgData GetDbgData(ActionFrame frame) { return DbgData.Empty; }
    }
}
