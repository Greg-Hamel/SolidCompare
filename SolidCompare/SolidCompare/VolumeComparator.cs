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
    public class VolumeComparator
    {
        static string path1, path2;
        static AssemblyDoc swAsbly;
        static Component2 swComp;
        static SldWorks.SldWorks swApp = Program.swApp;

        public VolumeComparator()
        {

        }

        public VolumeComparator(string componentPath1, string componentPath2)
        {
            path1 = componentPath1;
            path2 = componentPath2;
        }

        static bool CreateAssembly()
        {
            swAsbly = (AssemblyDoc)swApp.NewDocument(@"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\data\templates\assem.asmdot", 0, 0, 0);

            if (swAsbly == null)
            {
                Logger.Warn("Assembly document creation was unsuccessful.");
                return false;
            }
            else
            {
                Logger.Info("Assembly created.");
                return true;
            }

        }

        static bool InsertComponent(string path)
        {
            /* This will insert the componant into 
             * the newly created assembly */

            // ModelDoc2 tmpComponent = (ModelDoc2)swApp.OpenDoc7();
            swComp = (Component2)swAsbly.AddComponent5(path1, (int)swAddComponentConfigOptions_e.swAddComponentConfigOptions_CurrentSelectedConfig, "", false, "", 0, 0, 0);

            if (swComp == null)
            {
                Logger.Warn("Component addition was unsuccessful.");
                return false;
            }
            else
            {
                Logger.Info("Component added.");
                return true;
            }
        }

        static void SaveAsPart()
        {

        }

        static double SubstractVolumeAB()
        {
            return 0;  // returns the yielded volume
        }

        static double SubstractVolumeBA()
        {
            return 0;  // returns the yielded volume
        }

        static double CommonVolume()
        {
            return 0;  // returns the yielded volume
        }

        static int CompareVolume()
        {

            /* returns 0 for no change in volume
             * returns 1 for negative change
             * returns 2 for positive change */
            return 0;
        }

        static int CompareFaces()
        {

            /* returns 0 for no change in number of faces
             * returns 1 for negative change
             * returns 2 for positive change */
            return 0;
        }

        static int CompareSurface()
        {
            /* returns 0 for no change in total surface
             * returns 1 for negative change
             * returns 2 for positive change */
            return 0;
        }
    }
}
