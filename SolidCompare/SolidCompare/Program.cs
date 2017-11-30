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
using SolidCompare.Comparators;
using SolidCompare.Entities;

namespace SolidCompare
{
    static class Program
    {
        public static SldWorks.SldWorks swApp;

        static void Main()
        {
            swApp = SwApp.Instance;  // Get SolidWorks
            swApp.Visible = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Launcher lcher = new Launcher();
            Logger.Info("SolidCompare Started");
            Application.Run(lcher);
            Logger.Info("Launcher closed.");

            string dir1 = lcher.directory1;
            string dir2 = lcher.directory2;

            // The following is used for testing of this branch only.

            // ModelDoc2 Part1 = SwApp.OpenFile(@"C:\Users\hameg\Documents\SWs\A.SLDPRT");
            // ModelDoc2 Part2 = SwApp.OpenFile(@"C:\Users\hameg\Documents\SWs\B.SLDPRT");

            // ModelDoc2 Part1 = SwApp.OpenFile(@"C:\Users\ap12770\Documents\part1.sldprt");
            // ModelDoc2 Part2 = SwApp.OpenFile(@"C:\Users\ap12770\Documents\part2.sldprt");

            // VolumeComparator result = new VolumeComparator();
            // result.Compare(Part1, Part2);

            Assembly refAssembly = new Assembly((IAssemblyDoc)SwApp.OpenFile(dir1));
            Assembly modAssembly = new Assembly((IAssemblyDoc)SwApp.OpenFile(dir2));

            CompareResult compareResult = refAssembly.CompareTo(modAssembly);

            Console.WriteLine(compareResult);

            Logger.EndReport();
            MessageBox.Show("Done");
        }

    }
}
