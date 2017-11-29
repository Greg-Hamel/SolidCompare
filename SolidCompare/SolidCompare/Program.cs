using System;
using System.Windows.Forms;
using SldWorks;
using SolidCompare.Entities;

namespace SolidCompare
{
    static class Program
    {
        public static SldWorks.SldWorks swApp;

        static void Main()
        {
            // TEMPORARY START - for tests 
            string refFileName = "C:\\SOLIDWORKS Training Files\\API Fundamentals\\Lesson11 - Notifications\\Exercises\\UJ_for_INT_r.sldasm";
            //string modifiedFileName = "C:\\SOLIDWORKS Training Files\\API Fundamentals\\Lesson11 - Notifications\\Exercises mod\\UJ_for_INT_r2.sldasm";
            string modifiedFileName = "C:\\SOLIDWORKS Training Files\\API Fundamentals\\Lesson11 - Notifications\\Exercises mod\\UJ_for_INT_modif.sldasm";
            //string modifiedFileName = "C:\\SOLIDWORKS Training Files\\API Fundamentals\\Lesson11 - Notifications\\Exercises mod\\UJ_for_INT_r2_similar.sldasm";

            swApp = SwApp.Instance;
            swApp.Visible = true;
            Assembly refAssembly = new Assembly((IAssemblyDoc)SwApp.OpenFile(refFileName));
            Assembly modAssembly = new Assembly((IAssemblyDoc)SwApp.OpenFile(modifiedFileName));
            var compareResult = refAssembly.CompareTo(modAssembly);
            Console.WriteLine(compareResult);
            // TEMPORARY END

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Launcher lcher = new Launcher();
            Logger.Info("SolidCompare Started");
            Application.Run(lcher);
            Logger.Info("Launcher closed.");

            string dir1 = lcher.directory1;
            string dir2 = lcher.directory2;

            //swApp = SwApp.Instance;  // Get SolidWorks
        }

    }
}
