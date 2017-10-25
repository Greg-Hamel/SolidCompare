using System;
using System.Runtime.InteropServices;
using SldWorks;
using SwConst;

namespace SolidCompare
{
    public class Assembly
    {
        private string directory;


        public Assembly()
        {

        }

        public Assembly(string dir)
        {
            directory = dir;
        }

        public string Directory
        {
            get { return directory; }
            set { directory = value; }
        }
    }
}
