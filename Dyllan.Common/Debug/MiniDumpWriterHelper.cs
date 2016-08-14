using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Dyllan.Common.Debug
{
    public static class MiniDumpWriterHelper
    {
        const string DateTimeFormat = "yyyyMMdd-HHmmss.fff";

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool Write(string fileNamePrefix, string directoryName, MiniDumpType options = MiniDumpType.MiniDumpWithFullMemory
            | MiniDumpType.MiniDumpWithFullMemoryInfo | MiniDumpType.MiniDumpWithDataSegs | MiniDumpType.MiniDumpWithUnloadedModules
            | MiniDumpType.MiniDumpWithThreadInfo | MiniDumpType.MiniDumpWithHandleData)
        {
            bool result = false;
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            string fileName = Path.Combine(directoryName, string.Format("{0}-{1}.dmp", fileNamePrefix, DateTime.Now.ToString(DateTimeFormat)));

            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    Process currentProcess = Process.GetCurrentProcess();
                    IntPtr currentProcessHandle = currentProcess.Handle;
                    uint currentProcessId = (uint)currentProcess.Id;

                    result = MiniDumpWriter.MiniDumpWriteDump(currentProcessHandle, currentProcessId, fs.SafeFileHandle.DangerousGetHandle(),
                        (uint)options, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                }
            }
            catch (Exception)
            {
                // WriteLog
            }

            return result;
        }
    }
}
