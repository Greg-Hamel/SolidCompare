﻿using System;
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
            swApp.Visible = true;

            // The following is used for testing of this branch only.

            // ModelDoc2 Part1 = SwApp.OpenFile(@"C:\Users\hameg\Documents\SWs\A.SLDPRT");
            // ModelDoc2 Part2 = SwApp.OpenFile(@"C:\Users\hameg\Documents\SWs\B.SLDPRT");

            ModelDoc2 Part1 = SwApp.OpenFile(@"C:\Users\hameg\Documents\SWs\Assemblies\Ass1.SLDASM");
            ModelDoc2 Part2 = SwApp.OpenFile(@"C:\Users\hameg\Documents\SWs\Assemblies\Ass2.SLDASM");

            VolumeComparator.Compare(Part1, Part2);


            Logger.EndReport();
            MessageBox.Show("Done");
        }

    }
}
