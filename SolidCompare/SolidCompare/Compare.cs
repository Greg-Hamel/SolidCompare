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

        private void DebugFormat(string fmt, params object[] p)
        {
            Debug.WriteLine(fmt, p); // this will select the right overload 
                                     // ... due to typeof(p)==object[]
        }

        public void compare()
        {
            // Will launch the sequence of events required by the software 

            SldWorks.SldWorks swApp;
            ModelDoc2 swModel;

            connect_Sldwks();  // Connect to Solidwork App
        }

        private void connect_Sldwks()
        {
            // Tries to get a handle of the Solidwork App.
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

        private void load_assemblies()
        {
            // Loads the two assemblies in dir1 and dir2
            Assembly Asmbl1 = new Assembly(dir1);
            Assembly Asmbl2 = new Assembly(dir2);
        }
    }
}
