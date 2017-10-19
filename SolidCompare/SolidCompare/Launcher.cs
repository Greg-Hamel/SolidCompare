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


namespace SolidCompare
{   
    public partial class Launcher : Form
    {
        public string directory1 { get; set; }
        public string directory2 { get; set; }

        public Launcher()
        {
            InitializeComponent();
        }

        private void CompareButton_Click(object sender, EventArgs e)
        {
            DebugFormat("Directory1: {0}", this.FieldAssemblyDirectory1.Text);
            DebugFormat("Directory2: {0}", this.FieldAssemblyDirectory2.Text);

            this.directory1 = this.FieldAssemblyDirectory1.Text;
            this.directory2 = this.FieldAssemblyDirectory2.Text;
            this.Close();
        }

        public static void DebugFormat(string fmt, params object[] p)
        {
            Debug.WriteLine(fmt, p); // this will select the right overload 
                                     // ... due to typeof(p)==object[]
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            
        }


    }
}
