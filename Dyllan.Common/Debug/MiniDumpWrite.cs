using System;
using System.Runtime.InteropServices;

namespace Dyllan.Common.Debug
{
    internal static class MiniDumpWriter
    {
        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        internal static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, IntPtr hFile, uint dumpType, IntPtr expParam,
            IntPtr userStreamParam, IntPtr callbackParam);
    }
}
