using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.Run(new Launcher());
            Debug.WriteLine("SolidCompare Started");
        }
    }

    class Assembly
    {
        private string directory;
        public Assembly(string dir)
        {
            Console.WriteLine("Creating Assembly based on {0}", dir);
            directory = dir;
        }
    }
}
