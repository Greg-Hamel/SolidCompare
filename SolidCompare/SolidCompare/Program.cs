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
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>

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
            Debug.WriteLine("test: {0}", dir1, null);
            Debug.WriteLine("test: {0}", dir2, null);

            Compare comparer = new Compare(dir1, dir2);
            comparer.compare();

            MessageBox.Show("Done");

        }

    }
}
