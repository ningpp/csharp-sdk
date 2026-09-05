// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using dotnet.xml.schema;

namespace dotnet.xml
{
    internal interface IValidationEventHandling
    {
        // This is a ValidationEventHandler, but it is not strongly typed due to dependencies on dotnet.xml.Schema
        object EventHandler { get; }

        // The exception is XmlSchemaException, but it is not strongly typed due to dependencies on dotnet.xml.Schema
        void SendEvent(Exception exception, XmlSeverityType severity);
    }
}
