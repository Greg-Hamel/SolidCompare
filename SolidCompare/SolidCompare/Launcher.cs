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

        private void CompareButton_Click(object sender, EventArgs e)
        {
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

            // The following code shall execute the comparison between the two specified fields.

        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            
        }
    }
}
