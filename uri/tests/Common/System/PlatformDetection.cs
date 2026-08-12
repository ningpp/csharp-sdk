using System.Runtime.InteropServices;

namespace dotnet.system
{
    public static class PlatformDetection
    {
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }
}
