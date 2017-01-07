using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyllan.Common
{
    public static class Utils
    {
        private static ConcurrentDictionary<string, object> fileLockers = new ConcurrentDictionary<string, object>();

        public static void WriteToFile(string savePath, string content, bool append)
        {
            if (string.IsNullOrWhiteSpace(savePath))
                return;

            FileInfo file = new FileInfo(savePath);
            object locker = fileLockers.GetOrAdd(file.FullName, new object());

            lock (locker)
            {
                if (!Directory.Exists(file.DirectoryName))
                {
                    Directory.CreateDirectory(file.DirectoryName);
                }

                using (StreamWriter sw = new StreamWriter(savePath, append, Encoding.UTF8))
                {
                    sw.WriteLine(content);
                }
                object tempObject;
                fileLockers.TryRemove(file.FullName, out tempObject);
            }
        }
    }
}
