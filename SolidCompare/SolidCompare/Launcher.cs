using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\SWs";
            openFileDialog1.Filter = "Assembly(*.asm)|*.sldasm|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("+++" + openFileDialog1.FileName);
                FieldAssemblyDirectory1.Text = openFileDialog1.FileName;
                FieldAssemblyDirectory1.ReadOnly = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();

            openFileDialog2.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\SWs";
            openFileDialog2.Filter = "Assembly(*.asm)|*.sldasm|All files (*.*)|*.*";
            openFileDialog2.FilterIndex = 1;
            openFileDialog2.RestoreDirectory = true;

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("+++" + openFileDialog2.FileName);
                FieldAssemblyDirectory2.Text = openFileDialog2.FileName;
                FieldAssemblyDirectory2.ReadOnly = true;
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
