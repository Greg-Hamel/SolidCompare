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
        static Component2 swComp1, swComp2;
        static SldWorks.SldWorks swApp = Program.swApp;
        static Feature MateFeature;
        static string programFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);

        static ModelDoc2 component1;
        static ModelDoc2 component2;

        static bool boolstat;

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
            InsertComponents();
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

        static void InsertComponents()
        {
            /* This will insert the components into the newly created
             *  assembly and mate them so their origins are aligned   */

            ModelDoc2 swModel = (ModelDoc2)swAsbly;

            // Add Component1 *TBD*
            Logger.Info("Adding first component to assembly...");
            swComp1 = swAsbly.AddComponent5(component1.GetPathName(), (int)swAddComponentConfigOptions_e.swAddComponentConfigOptions_CurrentSelectedConfig, "",
                false, "", 0, 0, 0);

            string Comp1Name = swComp1.Name2;

            if (swComp1 == null)
            {
                Logger.Warn("Component addition was unsuccessful.");
            }
            else
            {
                Logger.Info("Component added successfully.");
            }

            // Add Component2 *TBD*
            Logger.Info("Adding second component to assembly...");
            swComp2 = swAsbly.AddComponent5(component2.GetPathName(), (int)swAddComponentConfigOptions_e.swAddComponentConfigOptions_CurrentSelectedConfig, "",
                false, "", 0, 0, 0);

            string Comp2Name = swComp2.Name2;

            if (swComp2 == null)
            {
                Logger.Warn("Component addition was unsuccessful.");
            }
            else
            {
                Logger.Info("Component added successfully.");
            }

            Logger.Info("Adding Origin Mate...");

            swModel.ClearSelection();

            ModelDocExtension swDocExt = swModel.Extension;

            string MateName = "Aligned_Origins";
            string FirstSelection = "Point1@Origin@" + Comp1Name + "@" + swModel.GetTitle();
            string SecondSelection = "Point1@Origin@" + Comp2Name + "@" + swModel.GetTitle();

            boolstat = swDocExt.SelectByID2(FirstSelection, "EXTSKETCHPOINT", 0, 0, 0, false, 1, null, 0);
            MessageBox.Show("1");
            boolstat = swDocExt.SelectByID2(SecondSelection, "EXTSKETCHPOINT", 0, 0, 0, true, 1, null, 0);

            MateFeature = (Feature)swAsbly.AddMate5((int)swMateType_e.swMateCOINCIDENT, (int)swMateAlign_e.swMateAlignALIGNED,
                false, 0, 0, 0, 0, 0, 0, 0, 0, false, false, (int)swMateWidthOptions_e.swMateWidth_Centered, out int MateError);
            if (MateError != 1)
            {
                Logger.Error("VolumeComparator", "InsertComponent", "swAddMateError: "+MateError);
            }
            MateFeature.Name = MateName;

            swModel.ClearSelection();
            Logger.Info("Mate added: " + MateFeature.Name);
        }

        static void MateComponents()
        {

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
