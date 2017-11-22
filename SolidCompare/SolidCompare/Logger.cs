using System;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;


namespace SolidCompare
{
    class Logger
    {
        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;

        static int WarnNumber = 0;
        static int ErrorNumber = 0;

        static void AllocateConsole()
        {
            AllocConsole();
            IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
            FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            Encoding encoding = Encoding.GetEncoding(MY_CODE_PAGE);
            StreamWriter standardOutput = new StreamWriter(fileStream, encoding) { AutoFlush = true };
            Console.SetOut(standardOutput);
        }

        public static void Info(string message)
        {
            #if !DEBUG
            AllocateConsole()
            #endif

            Console.WriteLine("[INFO]    " + DateTime.Now.ToString("h:mm:ss tt") + "    " + message);
        }

        public static void Warn(string message)
        {
            #if !DEBUG
            AllocateConsole()
            #endif

            Console.WriteLine( "[WARNING] " + DateTime.Now.ToString("h:mm:ss tt") + "    " + message);
            WarnNumber += 1;
        }

        public static void Error(string className, string methodName, string message)
        {
            #if !DEBUG
            AllocateConsole()
            #endif

            Console.WriteLine("[ERROR]   '" + DateTime.Now.ToString("h:mm:ss tt") + "    " + message + "' in method: " + methodName + " of class: " + className);
            ErrorNumber += 1;

            Console.WriteLine("Shutting down...");
            Console.Read();
            Environment.Exit(1);
        }

        public static void EndReport()
        {
            Info("The current program concluded with the following: \n\t\t\t\t\t\t\t\t" 
                + WarnNumber + " Warnings\n\t\t\t\t\t\t\t\t" + ErrorNumber + " Errors");
        }
    }
}
