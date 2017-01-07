using NLog;
using System;
using WebSide.Common.Spider;

namespace WebSideScatch.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = string.Empty;
            if (args != null && args.Length == 1)
            {
                fileName = args[1];
            }
            else
            {
                Console.Write("Input File Path: ");
                fileName = Console.ReadLine();
            }

            CustomTask task = new CustomTask(fileName);
            task.ExecuteInSequence();

            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
