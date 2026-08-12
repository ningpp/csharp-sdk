using System;
using Xunit;

namespace dotnet.system
{
    internal static class AssertExtensions
    {
        public static T Throws<T>(string paramName, Action action) where T : ArgumentException
        {
            var ex = Assert.Throws<T>(action);
            if (paramName != null)
            {
                Assert.Equal(paramName, ex.ParamName);
            }
            return ex;
        }

        // Overload that accepts a message parameter (ignored in this stub)
        public static T Throws<T>(string paramName, string message, Action action) where T : ArgumentException
        {
            return Throws<T>(paramName, action);
        }
    }
}
