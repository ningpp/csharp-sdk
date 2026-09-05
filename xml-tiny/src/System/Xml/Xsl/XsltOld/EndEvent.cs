// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace dotnet.xml.Xsl.XsltOld
{
    using System;
    using System.Diagnostics;
    using dotnet.xml;
    using dotnet.xml.xpath;

    internal class EndEvent : Event
    {
        private XPathNodeType _nodeType;

        internal EndEvent(XPathNodeType nodeType)
        {
            Debug.Assert(nodeType != XPathNodeType.Namespace);
            _nodeType = nodeType;
        }

        public override bool Output(Processor processor, ActionFrame frame)
        {
            return processor.EndEvent(_nodeType);
        }
    }
}
