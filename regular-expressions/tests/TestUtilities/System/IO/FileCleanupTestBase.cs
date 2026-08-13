// Simplified FileCleanupTestBase for standalone test project

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.IO
{
    /// <summary>Base class for test classes that use temporary files that need to be cleaned up.</summary>
    public abstract class FileCleanupTestBase : IDisposable
    {
        private string fallbackGuid = Guid.NewGuid().ToString("N").Substring(0, 10);

        protected FileCleanupTestBase()
        {
            TestDirectory = Path.Combine(Path.GetTempPath(), GetType().Name + "_" + Path.GetRandomFileName());
            Directory.CreateDirectory(TestDirectory);
        }

        ~FileCleanupTestBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try { Directory.Delete(TestDirectory, recursive: true); }
            catch { }
        }

        protected string TestDirectory { get; }

        protected string GetTestFilePath(int? index = null, [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) =>
            Path.Combine(TestDirectory, GetTestFileName(index, memberName, lineNumber));

        protected string GetTestFileName(int? index = null, [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            string testFileName = GenerateTestFileName(index, memberName, lineNumber);
            string testFilePath = Path.Combine(TestDirectory, testFileName);

            const int maxLength = 260 - 5;

            int excessLength = testFilePath.Length - maxLength;

            if (excessLength > 0)
            {
                if (excessLength < memberName.Length + "...".Length)
                {
                    memberName = memberName.Substring(0, memberName.Length / 2 - excessLength / 2) + "..." + memberName.Substring(memberName.Length / 2 + excessLength / 2);
                    testFileName = GenerateTestFileName(index, memberName, lineNumber);
                }
                else
                {
                    return fallbackGuid;
                }
            }

            return testFileName;
        }

        private string GenerateTestFileName(int? index, string memberName, int lineNumber) =>
            string.Format(
                index.HasValue ? "{0}_{1}_{2}_{3}" : "{0}_{1}_{3}",
                memberName ?? "TestBase",
                lineNumber,
                index.GetValueOrDefault(),
                Guid.NewGuid().ToString("N").Substring(0, 8));
    }
}
