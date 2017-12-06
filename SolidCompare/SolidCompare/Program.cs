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
        public static Reporter report;

        [STAThread]
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

            report = new Reporter(SwApp.StripFile(dir1));

            List<ModelDoc2> previousDocs = SwApp.ListCurrentlyOpened();

            Assembly refAssembly = new Assembly((IAssemblyDoc)SwApp.OpenFile(dir1));
            Assembly modAssembly = new Assembly((IAssemblyDoc)SwApp.OpenFile(dir2));

            report.AddComparison(refAssembly.Title, modAssembly.Title);

            CompareResult compareResult = refAssembly.CompareTo(modAssembly);

            Console.WriteLine(compareResult);

            report.AddSection("Assembly Comparison");
            report.AddSubSection("Reported Changes");
            report.AddLine(compareResult.ToString());
            report.AddSection("Volumetric Comparison");
            report.PrintDelayed();

            Logger.EndReport();
            SwApp.Cleanup(previousDocs);
            MessageBox.Show("Done");
        }
    }
}
