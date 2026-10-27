#!/usr/bin/env python3
"""
Extract embedded byte arrays from TestData.g.cs into binary resource files,
then generate a new TestData.g.cs that loads data from embedded resources,
and update the .csproj to include the resource files as EmbeddedResource.

Usage:  python extract_testdata.py
"""

import re
import os
import sys

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
INPUT_FILE = os.path.join(SCRIPT_DIR, "TestData.g.cs")
CSPROJ_FILE = os.path.join(SCRIPT_DIR, "dotnet-xml.xmlcoretest-tests.csproj")
RESOURCE_DIR = os.path.join(SCRIPT_DIR, "TestDataResources")


# ---------------------------------------------------------------------------
# 1. Parse the original TestData.g.cs
# ---------------------------------------------------------------------------

def parse_file_mappings(content):
    """Parse GetDataFiles() to extract (filename, s_fileN) tuples."""
    mappings = []
    pattern = re.compile(
        r'yield\s+return\s+Tuple\.Create\(\s*'
        r'(?:@"([^"]+)"|Path\.Combine\(([^)]+)\))\s*,\s*'
        r'(s_file\d+)\s*\)'
    )
    for m in pattern.finditer(content):
        if m.group(1):
            filename = m.group(1)
        else:
            parts = re.findall(r'"([^"]*)"', m.group(2))
            filename = os.path.join(*parts) if parts else ""
        mappings.append((filename, m.group(3)))
    return mappings


def parse_byte_arrays(content):
    """Parse every  private static byte[] s_fileN = { 0x.., ... };  definition."""
    arrays = {}
    pattern = re.compile(
        r'private\s+static\s+byte\[\]\s+(s_file\d+)\s*=\s*\{([^}]*)\}',
        re.DOTALL,
    )
    for m in pattern.finditer(content):
        hex_vals = re.findall(r'0x([0-9A-Fa-f]{2})', m.group(2))
        arrays[m.group(1)] = bytes(int(h, 16) for h in hex_vals)
    return arrays


# ---------------------------------------------------------------------------
# 2. Write resource .bin files
# ---------------------------------------------------------------------------

def write_resource_files(mappings, arrays):
    os.makedirs(RESOURCE_DIR, exist_ok=True)
    for filename, var_name in mappings:
        data = arrays[var_name]
        path = os.path.join(RESOURCE_DIR, var_name + ".bin")
        with open(path, "wb") as f:
            f.write(data)
    print(f"Wrote {len(mappings)} .bin files to {RESOURCE_DIR}")


# ---------------------------------------------------------------------------
# 3. Generate the new TestData.g.cs
# ---------------------------------------------------------------------------

def generate_new_testdata_cs(mappings):
    lines = []
    lines.append("// Licensed to the .NET Foundation under one or more agreements.")
    lines.append("// The .NET Foundation licenses this file to you under the MIT license.")
    lines.append("// See the LICENSE file in the project root for more information.")
    lines.append("")
    lines.append("using System;")
    lines.append("using System.IO;")
    lines.append("using System.Collections.Generic;")
    lines.append("using System.Reflection;")
    lines.append("")
    lines.append("namespace XmlCoreTest.Common")
    lines.append("{")
    lines.append("    public static partial class FilePathUtil")
    lines.append("    {")
    lines.append("        private static Dictionary<string, byte[]> s_resourceCache = null;")
    lines.append("        private static readonly object s_resourceLock = new object();")
    lines.append("")
    lines.append("        private static void LoadResourcesIfNotYet()")
    lines.append("        {")
    lines.append("            lock (s_resourceLock)")
    lines.append("            {")
    lines.append("                if (s_resourceCache == null)")
    lines.append("                {")
    lines.append("                    s_resourceCache = new Dictionary<string, byte[]>();")
    lines.append("                    Assembly assembly = typeof(FilePathUtil).GetTypeInfo().Assembly;")
    lines.append("                    string[] resourceNames = assembly.GetManifestResourceNames();")
    lines.append("                    foreach (string fullName in resourceNames)")
    lines.append("                    {")
    lines.append("                        if (!fullName.EndsWith(\".bin\", StringComparison.OrdinalIgnoreCase))")
    lines.append("                            continue;")
    lines.append("                        string withoutExt = fullName.Substring(0, fullName.Length - 4);")
    lines.append("                        int lastDot = withoutExt.LastIndexOf('.');")
    lines.append("                        string key = lastDot >= 0 ? withoutExt.Substring(lastDot + 1) : withoutExt;")
    lines.append("                        using (Stream stream = assembly.GetManifestResourceStream(fullName))")
    lines.append("                        {")
    lines.append("                            if (stream != null)")
    lines.append("                            {")
    lines.append("                                byte[] buffer = new byte[stream.Length];")
    lines.append("                                stream.Read(buffer, 0, buffer.Length);")
    lines.append("                                s_resourceCache[key] = buffer;")
    lines.append("                            }")
    lines.append("                        }")
    lines.append("                    }")
    lines.append("                }")
    lines.append("            }")
    lines.append("        }")
    lines.append("")
    lines.append("        private static byte[] GetResourceBytes(string resourceName)")
    lines.append("        {")
    lines.append("            LoadResourcesIfNotYet();")
    lines.append("            byte[] result;")
    lines.append("            if (!s_resourceCache.TryGetValue(resourceName, out result))")
    lines.append("                throw new FileNotFoundException(\"Embedded resource not found: \" + resourceName);")
    lines.append("            return result;")
    lines.append("        }")
    lines.append("")
    lines.append("        static IEnumerable<Tuple<string, byte[]>> GetDataFiles()")
    lines.append("        {")

    # Reproduce every yield-return line, replacing s_fileN with GetResourceBytes("s_fileN")
    for filename, var_name in mappings:
        # Rebuild the original-style line.
        # Determine whether the original used @"..." or Path.Combine(...)
        # We always use Path.Combine style for consistency, except for simple names.
        if "/" in filename or "\\" in filename:
            # Use Path.Combine
            parts = re.split(r'[\\/]', filename)
            parts_quoted = ', '.join('"' + p + '"' for p in parts)
            line = f'            yield return Tuple.Create(Path.Combine({parts_quoted}), GetResourceBytes("{var_name}"));'
        else:
            line = f'            yield return Tuple.Create(@"{filename}", GetResourceBytes("{var_name}"));'
        lines.append(line)

    lines.append("        }")
    lines.append("    }")
    lines.append("}")
    lines.append("")
    return '\n'.join(lines)


# ---------------------------------------------------------------------------
# 4. Update .csproj
# ---------------------------------------------------------------------------

def update_csproj():
    with open(CSPROJ_FILE, "r", encoding="utf-8-sig") as f:
        content = f.read()

    if "TestDataResources" in content:
        print("csproj already references TestDataResources, skipping.")
        return

    insertion = (
        "\n  <ItemGroup>\n"
        "    <EmbeddedResource Include=\"TestDataResources\\*.bin\" />\n"
        "  </ItemGroup>\n"
    )
    content = content.replace("</Project>", insertion + "</Project>")
    with open(CSPROJ_FILE, "w", encoding="utf-8") as f:
        f.write(content)
    print(f"Updated {CSPROJ_FILE} with EmbeddedResource entries.")


# ---------------------------------------------------------------------------
# main
# ---------------------------------------------------------------------------

def main():
    print(f"Reading {INPUT_FILE} ...")
    with open(INPUT_FILE, "r", encoding="utf-8-sig") as f:
        content = f.read()

    print("Parsing file mappings ...")
    mappings = parse_file_mappings(content)
    print(f"  {len(mappings)} mappings found.")

    print("Parsing byte arrays ...")
    arrays = parse_byte_arrays(content)
    print(f"  {len(arrays)} byte arrays found.")

    # sanity check
    for fn, var in mappings:
        if var not in arrays:
            sys.exit(f"ERROR: {var} (for {fn}) not found among byte arrays!")

    print("Writing resource files ...")
    write_resource_files(mappings, arrays)

    print("Generating new TestData.g.cs ...")
    new_cs = generate_new_testdata_cs(mappings)
    with open(INPUT_FILE, "w", encoding="utf-8") as f:
        f.write(new_cs)
    print(f"  {INPUT_FILE} rewritten ({len(new_cs)} bytes).")

    print("Updating .csproj ...")
    update_csproj()

    print("\nAll done!")


if __name__ == "__main__":
    main()
