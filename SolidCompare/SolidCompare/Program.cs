using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using SldWorks;
using SwConst;

namespace SolidCompare
{
    static class Program
    {
        public static SldWorks.SldWorks swApp;

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Launcher lcher = new Launcher();
            Logger.Info("SolidCompare Started");
            Application.Run(lcher);
            Logger.Info("Launcher closed.");

            string dir1 = lcher.directory1;
            string dir2 = lcher.directory2;

            swApp = SwApp.Instance;  // Get SolidWorks
        }

    }
}
