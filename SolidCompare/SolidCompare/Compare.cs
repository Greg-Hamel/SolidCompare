using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SldWorks;
using SwConst;
using System.Diagnostics;

namespace SolidCompare
{
    public class Compare
    {
        public string dir1, dir2;

        public Compare(string directory1, string directory2)
        {
            dir1 = directory1;
            dir2 = directory2;

            DebugFormat("Directory1: {0}", dir1);
            DebugFormat("Directory2: {0}", dir2);
        }

        public void start()
        {
            SldWorks.SldWorks swApp;
            // ModelDoc2 swModel;

            try
            {
                swApp = (SldWorks.SldWorks)Marshal.GetActiveObject("SldWorks.Application");  // Assign SolidWork window to swApp
            }
            catch
            {
                MessageBox.Show("Cannot get handle of SolidWorks.");  // Couldn't find the SolidWorks window.
                return;
            }

            Debug.WriteLine("Handle of SolidWorks was established.");
        }


        public static void DebugFormat(string fmt, params object[] p)
        {
            Debug.WriteLine(fmt, p); // this will select the right overload 
                                     // ... due to typeof(p)==object[]
        }

    }
}
