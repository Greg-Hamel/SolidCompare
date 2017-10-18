using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SldWorks;
using SwConst;

namespace SolidCompare
{   
    public partial class Launcher : Form
    {
        SldWorks.SldWorks swApp;
        ModelDoc2 swModel;

        public Launcher()
        {
            InitializeComponent();
            Debug.WriteLine("Starting SolidCompare");
        }

        public static void DebugFormat(string fmt, params object[] p)
        {
            Debug.WriteLine(fmt, p); // this will select the right overload 
                                     // ... due to typeof(p)==object[]
        }

        private void CompareButton_Click(object sender, EventArgs e)
        {
            string directory1;
            string directory2;

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

            directory1 = this.FieldAssemblyDirectory1.Text;
            directory2 = this.FieldAssemblyDirectory2.Text;

            DebugFormat("Directory1: {0}", directory1);
            DebugFormat("Directory2: {0}", directory2);

            // The following code shall execute the comparison between the two specified fields.

        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            
        }


    }
}
