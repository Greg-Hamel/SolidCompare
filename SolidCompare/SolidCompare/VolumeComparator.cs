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
        static Feature mateFeature;
        static ModelDoc2 comparePart;

        static string programFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);

        static ModelDoc2 component1;
        static ModelDoc2 component2;

        static bool boolstat;
        static string componentPath;
        static string componentFolder;
        static string comp1Name, comp2Name;
        static string comparePartName;
        static string comparePartPath;

        public VolumeComparator()
        {

        }

        public static void Compare(ModelDoc2 comparer, ModelDoc2 comparee)
        {
            // This is the main method to execute the rest of the comparison.
            

            component1 = comparer;
            component2 = comparee;

            GetInfo();
            CheckComponents();
            CreateAssembly();
            InsertComponents();
            SaveAsPart();
            ClosePreviouslyOpenedDocs();

            comparePart = SwApp.OpenFile(comparePartPath);

        }

        static void GetInfo()
        {
            string compTitle;
            string[] strings;
            int index;

            componentPath = component2.GetPathName();

            compTitle = component1.GetTitle();
            strings = compTitle.Split(new Char[] { '.' });
            comp1Name = strings[0];

            compTitle = component2.GetTitle();
            strings = compTitle.Split(new Char[] { '.' });
            comp2Name = strings[0];

            index = componentPath.IndexOf(compTitle);

            componentFolder =  componentPath.Substring(0, (index));
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
            ModelDocExtension swDocExt = swModel.Extension;

            int mateError;
            string mateName;
            string firstSelection;
            string secondSelection;

            // Add Component1
            Logger.Info("Adding first component to assembly...");
            swComp1 = swAsbly.AddComponent5(component1.GetPathName(), (int)swAddComponentConfigOptions_e.swAddComponentConfigOptions_CurrentSelectedConfig, "",
                false, "", 0, 0, 0);

            string comp1Name = swComp1.Name2;

            if (swComp1 == null)
            {
                Logger.Warn("Component addition was unsuccessful.");
            }
            else
            {
                Logger.Info("Component added successfully.");
            }

            // Add Component2
            Logger.Info("Adding second component to assembly...");
            swComp2 = swAsbly.AddComponent5(component2.GetPathName(), (int)swAddComponentConfigOptions_e.swAddComponentConfigOptions_CurrentSelectedConfig, "",
                false, "", 0, 0, 0);

            string comp2Name = swComp2.Name2;

            if (swComp2 == null)
            {
                Logger.Warn("Component addition was unsuccessful.");
            }
            else
            {
                Logger.Info("Component added successfully.");
            }


            // Add Mate 1
            Logger.Info("Adding Origin Mate...");

            swModel.ClearSelection();

            mateName = "Aligned_Origins";
            firstSelection = "Point1@Origin@" + comp1Name + "@" + swModel.GetTitle();
            secondSelection = "Point1@Origin@" + comp2Name + "@" + swModel.GetTitle();

            boolstat = swDocExt.SelectByID2(firstSelection, "EXTSKETCHPOINT", 0, 0, 0, false, 1, null, 0);
            boolstat = swDocExt.SelectByID2(secondSelection, "EXTSKETCHPOINT", 0, 0, 0, true, 1, null, 0);

            mateFeature = (Feature)swAsbly.AddMate5((int)swMateType_e.swMateCOINCIDENT, (int)swMateAlign_e.swMateAlignALIGNED,
                false, 0, 0, 0, 0, 0, 0, 0, 0, false, false, (int)swMateWidthOptions_e.swMateWidth_Centered, out mateError);
            if (mateError != 1)
            {
                Logger.Error("VolumeComparator", "InsertComponent", "swAddMateError: "+mateError);
            }
            mateFeature.Name = mateName;

            swModel.ClearSelection();
            Logger.Info("Mate added: " + mateFeature.Name);

            // Add Mate 2

            Logger.Info("Adding Top Plane Mate...");

            swModel.ClearSelection();

            mateName = "Aligned_Top";
            firstSelection = "Top@" + comp1Name + "@" + swModel.GetTitle();
            secondSelection = "Top@" + comp2Name + "@" + swModel.GetTitle();

            boolstat = swDocExt.SelectByID2(firstSelection, "PLANE", 0, 0, 0, false, 1, null, 0);
            boolstat = swDocExt.SelectByID2(secondSelection, "PLANE", 0, 0, 0, true, 1, null, 0);

            mateFeature = (Feature)swAsbly.AddMate5((int)swMateType_e.swMateCOINCIDENT, (int)swMateAlign_e.swMateAlignALIGNED,
                false, 0, 0, 0, 0, 0, 0, 0, 0, false, false, (int)swMateWidthOptions_e.swMateWidth_Centered, out mateError);
            if (mateError != 1)
            {
                Logger.Error("VolumeComparator", "InsertComponent", "swAddMateError: " + mateError);
            }
            mateFeature.Name = mateName;

            swModel.ClearSelection();
            Logger.Info("Mate added: " + mateFeature.Name);

            // Add Mate 3

            Logger.Info("Adding Front Plane Mate...");

            swModel.ClearSelection();

            mateName = "Aligned_Front";
            firstSelection = "Front@" + comp1Name + "@" + swModel.GetTitle();
            secondSelection = "Front@" + comp2Name + "@" + swModel.GetTitle();

            boolstat = swDocExt.SelectByID2(firstSelection, "PLANE", 0, 0, 0, false, 1, null, 0);
            boolstat = swDocExt.SelectByID2(secondSelection, "PLANE", 0, 0, 0, true, 1, null, 0);

            mateFeature = (Feature)swAsbly.AddMate5((int)swMateType_e.swMateCOINCIDENT, (int)swMateAlign_e.swMateAlignALIGNED,
                false, 0, 0, 0, 0, 0, 0, 0, 0, false, false, (int)swMateWidthOptions_e.swMateWidth_Centered, out mateError);
            if (mateError != 1)
            {
                Logger.Error("VolumeComparator", "InsertComponent", "swAddMateError: " + mateError);
            }
            mateFeature.Name = mateName;

            swModel.ClearSelection();
            Logger.Info("Mate added: " + mateFeature.Name);

        }

        static void SaveAsPart()
        {
            ModelDoc2 swModel = (ModelDoc2)swAsbly;
            int errors = 0;
            int warnings = 0;

            comparePartName =  "Compare_" + comp1Name + "_&_" + comp2Name + ".sldprt";
            comparePartPath = componentFolder + comparePartName;

            Logger.Info("Saving Assembly as '.SLDPRT'...");
            swModel.SaveAs4(comparePartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, errors, warnings);

            if (errors != 0) { Logger.Error("VolumeComparator", "SaveAsPart", "swSaveAsError: " + errors); }
            else if (warnings != 0) { Logger.Warn("swFileSaveWarning: " + warnings); }
            else
            {
                Logger.Info("Save successful.");
            }
        }

        static void ClosePreviouslyOpenedDocs()
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
