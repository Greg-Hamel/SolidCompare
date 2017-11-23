using System;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SldWorks;
using SwConst;

namespace SolidCompare
{
    public class VolumeComparator
    {
        static AssemblyDoc swAsbly;
        static Component2 swComp1, swComp2;
        static SldWorks.SldWorks swApp = Program.swApp;
        static Feature mateFeature;
        static ModelDoc2 comparePart;
        static Configuration config1, config2, config3;
        static string programFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);

        static ModelDoc2 component1;
        static ModelDoc2 component2;

        static object[] bodies;
        static Body2 body1, body2;

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
            bodies = ((PartDoc)comparePart).GetBodies2((int)swBodyType_e.swSolidBody, true);
            body1 = (Body2)bodies[0];
            body2 = (Body2)bodies[1];
            Logger.Info("Volume Compare:\t" + CompareVolume());
            Logger.Info("Area Compare:\t" + CompareArea());
            Logger.Info("Faces Compare:\t" + CompareFaces());
            CreateConfigurations();
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
            string AsblyTitle;

            Logger.Info("Creating the assembly...");
            swAsbly = (AssemblyDoc)swApp.NewDocument(programFile + @"\SOLIDWORKS Corp\SOLIDWORKS\data\templates\assem.asmdot", 0, 0, 0);

            AsblyTitle = ((ModelDoc2)swAsbly).GetTitle();

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
            swApp.CloseDoc(((ModelDoc2)swAsbly).GetTitle());
            swApp.CloseDoc(component1.GetTitle());
            swApp.CloseDoc(component2.GetTitle());
        }

        static void CreateConfigurations()
        {
            Logger.Info("Creating three (3) configurations...");
            config1 = comparePart.AddConfiguration3(comp1Name+"_minus_"+ comp2Name, 
                "Substraction of body1 minus body2", "", (int)swConfigurationOptions2_e.swConfigOption_DontActivate);
            config2 = comparePart.AddConfiguration3(comp2Name + "_minus_" + comp1Name,
                "Substraction of body2 minus body1", "", (int)swConfigurationOptions2_e.swConfigOption_DontActivate);
            config3 = comparePart.AddConfiguration3(comp1Name + "_and_" + comp2Name,
                "Combination of body1 and body2", "", (int)swConfigurationOptions2_e.swConfigOption_DontActivate);

            if (config1 != null || config2 != null | config3 != null)
            {
                Logger.Info("All configurations created successfully.");
            }
            else
            {
                Logger.Error("VolumeComparator", "CreateConfigurations()", "Something went wrong in the creation of the configurations.");
            }
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
            double[] body1MassProp, body2MassProp;
            double body1Volume, body2Volume;

            body1MassProp = body1.GetMassProperties(0);  // Using a densities of '0' since we don't need mass
            body2MassProp = body2.GetMassProperties(0);

            body1Volume = Math.Round(body1MassProp[3], 8);
            body2Volume = Math.Round(body2MassProp[3], 8);

            Logger.Info("Volume A:" + body1Volume + "\tVolume B:" + body2Volume);

            return IncDecDoubleReport(body1Volume, body2Volume, "CompareVolume()");
        }

        static int CompareFaces()
        {
            /* returns 0 for no change in number of faces
             * returns 1 for negative change
             * returns 2 for positive change */

            int body1Faces, body2Faces;

            body1Faces = body1.GetFaceCount();
            body2Faces = body2.GetFaceCount();


            Logger.Info("Faces A:" + body1Faces + "\tFaces B:" + body2Faces);

            return IncDecIntReport(body1Faces, body2Faces, "CompareFaces()");
        }

        static int CompareArea()
        {
            /* returns 0 for no change in total surface
             * returns 1 for negative change
             * returns 2 for positive change */

            object[] body1Faces, body2Faces;
            double body1Area, body2Area;


            body1Faces = body1.GetFaces();
            body2Faces = body2.GetFaces();

            body1Area = 0;
            foreach (Face2 face in body1Faces)
            {
                body1Area += face.GetArea();
            }

            body2Area = 0;
            foreach (Face2 face in body2Faces)
            {
                body2Area += face.GetArea();
            }

            body1Area = Math.Round(body1Area, 8);
            body2Area = Math.Round(body2Area, 8);

            Logger.Info("Area A:" + body1Area + "\tArea B:" + body2Area);

            return IncDecDoubleReport(body1Area, body2Area, "CompareArea()");
        }
        static int IncDecDoubleReport(double number1, double number2, string methodname)
        {
            if (number1 == number2) { return 0; }
            else if (number1 > number2) { return 1; }
            else if (number1 < number2) { return 2; }
            else
            {
                Logger.Error("VolumeComparator", methodname, "Unknown case has happened...");
                return -1;
            }
        }

        static int IncDecIntReport(int number1, int number2, string methodname)
        {
            if (number1 == number2) { return 0; }
            else if (number1 > number2) { return 1; }
            else if (number1 < number2) { return 2; }
            else
            {
                Logger.Error("VolumeComparator", methodname, "Unknown case has happened...");
                return -1;
            }
        }
    }
}
