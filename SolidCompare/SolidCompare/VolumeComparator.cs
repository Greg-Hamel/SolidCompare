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
        static AssemblyDoc swAsbly;
        static Component2 swComp;
        static SldWorks.SldWorks swApp = Program.swApp;
        static string programFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);

        static ModelDoc2 component1;
        static ModelDoc2 component2;

        public VolumeComparator()
        {

        }

        public static void Compare(ModelDoc2 comparer, ModelDoc2 comparee)
        {
            // This is the main method to execute the rest of the comparison.
            component1 = comparer;
            component2 = comparee;
            
            CheckComponents();
            CreateAssembly();
        }

        static bool CreateAssembly()
        {
            Logger.Info("Creating the assembly...");
            swAsbly = (AssemblyDoc)swApp.NewDocument(programFile + @"\SOLIDWORKS Corp\SOLIDWORKS\data\templates\assem.asmdot", 0, 0, 0);

            string AsblyTitle = ((ModelDoc2)swAsbly).GetTitle();

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

        static void CheckComponents()
        {
            Logger.Info("Components are being verified...");
            if (component1.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY || component2.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                Logger.Info("Components are both of Assembly Type");
            }
            else if (component1.GetType() == (int)swDocumentTypes_e.swDocPART || component2.GetType() == (int)swDocumentTypes_e.swDocPART)
            {
                Logger.Info("Components are both of Part Type");
            }
            else if (component1.GetType() == (int)swDocumentTypes_e.swDocDRAWING || component2.GetType() == (int)swDocumentTypes_e.swDocDRAWING)
            {
                Logger.Error("VolumeComparator", "InsertComponent", "Drawing Document provided, but not supported.");
            }
            else if (component1.GetType() != component2.GetType())
            {
                Logger.Error("VolumeComparator", "InsertComponent", "Two different type of document are provided.");
            }
            else
            {
                Logger.Error("VolumeComparator", "InsertComponent", "Unexpected condition was met.");
            }
        }

        static bool InsertComponents()
        {
            /* This will insert the components into 
             * the newly created assembly */

            // Add Component1 *TBD*
            swComp = swAsbly.AddComponent5(path, (int)swAddComponentConfigOptions_e.swAddComponentConfigOptions_CurrentSelectedConfig, "", false, "", 0, 0, 0);

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

            // Add Component2 *TBD*
            swComp = swAsbly.AddComponent5(path, (int)swAddComponentConfigOptions_e.swAddComponentConfigOptions_CurrentSelectedConfig, "", false, "", 0, 0, 0);

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
