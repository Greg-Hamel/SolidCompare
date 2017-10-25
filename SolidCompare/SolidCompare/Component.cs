using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCompare
{
    class Component
    {
        private string directory;

        public Component()
        {

        }

        public Component(string dir)
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
