// Simplified RemoteExecutor stub for standalone test project

using System;

namespace Microsoft.DotNet.RemoteExecutor
{
    /// <summary>
    /// Stub implementation of RemoteExecutor for standalone test project.
    /// In the actual .NET runtime, this would run code in a separate process.
    /// For our standalone version, we just invoke the action in the current process.
    /// </summary>
    public static class RemoteExecutor
    {
        public static bool IsSupported => false;

        public static RemoteInvokeInvoke Invoke(Action action)
        {
            return new RemoteInvokeInvoke(action);
        }

        public static RemoteInvokeInvoke<T> Invoke<T>(Action<T> action, T arg)
        {
            return new RemoteInvokeInvoke<T>(action, arg);
        }
    }

    public class RemoteInvokeInvoke : IDisposable
    {
        private readonly Action _action;

        public RemoteInvokeInvoke(Action action)
        {
            _action = action;
        }

        public int ExitCode { get; private set; }

        public string StdOut => string.Empty;

        public string StdErr => string.Empty;

        public void Dispose()
        {
            try
            {
                _action();
                ExitCode = 0;
            }
            catch
            {
                ExitCode = 1;
            }
        }
    }

    public class RemoteInvokeInvoke<T> : IDisposable
    {
        private readonly Action<T> _action;
        private readonly T _arg;

        public RemoteInvokeInvoke(Action<T> action, T arg)
        {
            _action = action;
            _arg = arg;
        }

        public int ExitCode { get; private set; }

        public string StdOut => string.Empty;

        public string StdErr => string.Empty;

        public void Dispose()
        {
            try
            {
                _action(_arg);
                ExitCode = 0;
            }
            catch
            {
                ExitCode = 1;
            }
        }
    }
}
