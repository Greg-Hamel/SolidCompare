using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SldWorks;
using SwConst;

namespace SolidCompare
{
    static class Program
    {

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Launcher lcher = new Launcher();
            Debug.WriteLine("SolidCompare Started");
            Application.Run(lcher);
            Debug.WriteLine("Launcher closed.");

            string dir1 = lcher.directory1;
            string dir2 = lcher.directory2;


            MessageBox.Show("Done");

        }

    }
}
