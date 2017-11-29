using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SldWorks;
using SwConst;

namespace SolidCompare
{
    public class Info
    {
        // This class allows the storage of variable value in dictionnary.
        public object Value { get; set; }
    }

    public class VolumeComparator
    {
        static Component2 swComp1, swComp2;
        static SldWorks.SldWorks swApp = Program.swApp;
        static Feature mateFeature;
        static Configuration config1, config2, config3;
        static string programFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);


        static bool boolstat;
        static string comparePartName;
        static string comparePartPath; 

        public VolumeComparator()
        {

        }

        public static void Compare(ModelDoc2 comparer, ModelDoc2 comparee)
        {
            // This is the main method to execute the rest of the comparison.
            ModelDoc2 component1;
            ModelDoc2 component2;

            component1 = comparer;
            component2 = comparee;

            StartComparison(component1, component2);
        }

        public static void Compare(Component2 comparer, Component2 comparee)
        {
            // This is the main method to execute the rest of the comparison.
            ModelDoc2 component1;
            ModelDoc2 component2;

            component1 = (ModelDoc2)comparer;
            component2 = (ModelDoc2)comparee;

            StartComparison(component1, component2);
        }

        static void StartComparison(ModelDoc2 component1, ModelDoc2 component2)
        {
            AssemblyDoc swAsbly;
            int volumeDiff, faceDiff, areaDiff;
            double aMinusB, bMinusA, aAndB;
            object[] bodies;
            Body2 body1, body2;
            ModelDoc2 comparePart;
            int comparisonType;
            double volumeA, volumeB;

            Dictionary<string, Info> component1Info;
            Dictionary<string, Info> component2Info;

            component1Info = GetInfo(component1);
            component2Info = GetInfo(component2);
            comparisonType = CheckComponents(component1, component1Info, component2, component2Info);
            if (comparisonType == 1)
            {
                component1 = Convert2Part(component1, component1Info);
                component2 = Convert2Part(component2, component2Info);
            }


            swAsbly = CreateAssembly();
            InsertComponents(swAsbly, component1, component2);
            SaveAsPart(swAsbly, component1Info, component2Info);
            CloseDocs(new object[] { swAsbly, component1, component2 });

            comparePart = SwApp.OpenFile(comparePartPath);
            bodies = ((PartDoc)comparePart).GetBodies2((int)swBodyType_e.swSolidBody, true);
            body1 = (Body2)bodies[0];
            body2 = (Body2)bodies[1];

            volumeA = (body1.GetMassProperties(0))[3];
            volumeB = (body2.GetMassProperties(0))[3];

            faceDiff = CompareFaces(body1, body2);
            areaDiff = CompareArea(body1, body2);
            volumeDiff = CompareVolume(body1, body2);
            Logger.Info("Volume Compare:\t" + volumeDiff);
            Logger.Info("Area Compare:\t" + areaDiff);
            Logger.Info("Faces Compare:\t" + faceDiff);

            aMinusB = SubstractVolume(comparePart, body1, body2);
            Logger.Info("Volume A-B: " + aMinusB);
            bMinusA = SubstractVolume(comparePart, body2, body1);
            Logger.Info("Volume B-A: " + aMinusB);
            aAndB = CommonVolume(comparePart, body1, body2);
            Logger.Info("Volume B&A: " + aAndB);
        }

        static Dictionary<string, Info> GetInfo(ModelDoc2 component)
        {
            Dictionary<string, Info> infoDict = new Dictionary<string, Info>()
            {
                {"Path", new Info()},   // This is the full path including name and extension.
                {"Title", new Info()},  // This is the title of the document including the extension.
                {"Name", new Info() },  // This is the title of the document excluding the extension.
                {"Folder", new Info()},  // This is the full path excluing name and extension.
                {"Type", new Info()}     
            };

            string[] strings;
            int index;

            infoDict["Path"].Value = component.GetPathName();

            infoDict["Title"].Value = component.GetTitle();
            strings = ((string)infoDict["Title"].Value).Split(new Char[] { '.' });
            infoDict["Name"].Value = strings[0];

            index = ((string)infoDict["Path"].Value).IndexOf((string)infoDict["Title"].Value);

            infoDict["Folder"].Value = ((string)infoDict["Path"].Value).Substring(0, (index));

            infoDict["Type"].Value = component.GetType();

            return infoDict;
        }

        static AssemblyDoc CreateAssembly()
        {
            string AsblyTitle;
            AssemblyDoc newAssembly;

            Logger.Info("Creating the assembly...");
            newAssembly = (AssemblyDoc)swApp.NewDocument(programFile + @"\SOLIDWORKS Corp\SOLIDWORKS\data\templates\assem.asmdot", 0, 0, 0);

            AsblyTitle = ((ModelDoc2)newAssembly).GetTitle();

            if (newAssembly == null)
            {
                Logger.Warn("Assembly document creation was unsuccessful.");
                return null;
            }
            else
            {
                Logger.Info("Assembly created.");
                return newAssembly;
            }

        }

        static int CheckComponents(ModelDoc2 component1, Dictionary<string, Info> comp1Info, ModelDoc2 component2, Dictionary<string, Info> comp2Info)
        {
            
            Logger.Info("Components are being verified...");
            if ((int)comp1Info["Type"].Value == (int)swDocumentTypes_e.swDocASSEMBLY || (int)comp2Info["Type"].Value == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                Logger.Info("Components are both of Assembly Type");
                return 1;  // Assemblies
            }
            else if ((int)comp1Info["Type"].Value == (int)swDocumentTypes_e.swDocPART || (int)comp2Info["Type"].Value == (int)swDocumentTypes_e.swDocPART)
            {
                Logger.Info("Components are both of Part Type");
                return 2;  // Parts
            }
            else if ((int)comp1Info["Type"].Value == (int)swDocumentTypes_e.swDocDRAWING || (int)comp2Info["Type"].Value == (int)swDocumentTypes_e.swDocDRAWING)
            {
                Logger.Error("VolumeComparator", "InsertComponent", "Drawing Document provided, but not supported.");
                return 0;
            }
            else if ((int)comp1Info["Type"].Value != (int)comp2Info["Type"].Value)
            {
                Logger.Error("VolumeComparator", "InsertComponent", "Two different type of document are provided.");
                return 0;
            }
            else
            {
                Logger.Error("VolumeComparator", "InsertComponent", "Unexpected condition was met.");
                return 0;
            }
        }

        static void InsertComponents(AssemblyDoc assembly, ModelDoc2 component1, ModelDoc2 component2)
        {
            /* This will insert the components into the newly created
             *  assembly and mate them so their origins are aligned   */

            ModelDoc2 swModel = (ModelDoc2)assembly;
            ModelDocExtension swDocExt = swModel.Extension;

            int mateError;
            string mateName;
            string firstSelection;
            string secondSelection;

            // Add Component1
            Logger.Info("Adding first component to assembly...");
            swComp1 = assembly.AddComponent5(component1.GetPathName(), (int)swAddComponentConfigOptions_e.swAddComponentConfigOptions_CurrentSelectedConfig, "",
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
            swComp2 = assembly.AddComponent5(component2.GetPathName(), (int)swAddComponentConfigOptions_e.swAddComponentConfigOptions_CurrentSelectedConfig, "",
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

            mateFeature = (Feature)assembly.AddMate5((int)swMateType_e.swMateCOINCIDENT, (int)swMateAlign_e.swMateAlignALIGNED,
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

            Logger.Info(firstSelection);
            Logger.Info(secondSelection);

            boolstat = swDocExt.SelectByID2(firstSelection, "PLANE", 0, 0, 0, false, 1, null, 0);
            boolstat = swDocExt.SelectByID2(secondSelection, "PLANE", 0, 0, 0, true, 1, null, 0);

            if (!boolstat)
            {
                firstSelection = "Top Plane@" + comp1Name + "@" + swModel.GetTitle();
                secondSelection = "Top Plane@" + comp2Name + "@" + swModel.GetTitle();
                boolstat = swDocExt.SelectByID2(firstSelection, "PLANE", 0, 0, 0, false, 1, null, 0);
                boolstat = swDocExt.SelectByID2(secondSelection, "PLANE", 0, 0, 0, true, 1, null, 0);
            }

            mateFeature = (Feature)assembly.AddMate5((int)swMateType_e.swMateCOINCIDENT, (int)swMateAlign_e.swMateAlignALIGNED,
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

            if (!boolstat)
            {
                firstSelection = "Front Plane@" + comp1Name + "@" + swModel.GetTitle();
                secondSelection = "Front Plane@" + comp2Name + "@" + swModel.GetTitle();
                boolstat = swDocExt.SelectByID2(firstSelection, "PLANE", 0, 0, 0, false, 1, null, 0);
                boolstat = swDocExt.SelectByID2(secondSelection, "PLANE", 0, 0, 0, true, 1, null, 0);
            }

            mateFeature = (Feature)assembly.AddMate5((int)swMateType_e.swMateCOINCIDENT, (int)swMateAlign_e.swMateAlignALIGNED,
                false, 0, 0, 0, 0, 0, 0, 0, 0, false, false, (int)swMateWidthOptions_e.swMateWidth_Centered, out mateError);
            if (mateError != 1)
            {
                Logger.Error("VolumeComparator", "InsertComponent", "swAddMateError: " + mateError);
            }
            mateFeature.Name = mateName;

            swModel.ClearSelection();
            Logger.Info("Mate added: " + mateFeature.Name);

        }

        static void SaveAsPart(AssemblyDoc assembly, Dictionary<string, Info> comp1info, Dictionary<string, Info> comp2info)
        {
            // Used when creating the comparing assembly.
            ModelDoc2 model = (ModelDoc2)assembly;
            int errors = 0;
            int warnings = 0;

            comparePartName =  "Compare_" + comp1info["Name"].Value + "_&_" + comp2info["Name"].Value + ".sldprt";
            comparePartPath = comp1info["Folder"].Value + comparePartName;

            Logger.Info("Saving Assembly as '.SLDPRT'...");
            model.SaveAs4(comparePartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, errors, warnings);

            if (errors != 0) { Logger.Error("VolumeComparator", "SaveAsPart", "swSaveAsError: " + errors); }
            else if (warnings != 0) { Logger.Warn("swFileSaveWarning: " + warnings); }
            else
            {
                Logger.Info("Save successful.");
            }
        }

        static ModelDoc2 Convert2Part(ModelDoc2 assembly, Dictionary<string, Info> assemblyInfo)
        {
            // Used when comparing assemblies.
            ModelDoc2 model = assembly;
            ModelDocExtension modelExt = model.Extension;
            int errors = 0;
            int warnings = 0;
            string assemblyPartName;
            string assemblyPartPath;
            ModelDoc2 convertedAssembly;
            object[] bodies_array;
            bool check;

            if ((int)assemblyInfo["Type"].Value == (int)swDocumentTypes_e.swDocPART)
            {
                Logger.Error("VolumeComparator.cs", "Convert2Part()", "The document provided is not an assembly document.");
                return null;
            }

            assemblyPartName = assemblyInfo["Name"].Value + "_PartConversion.sldprt";
            assemblyPartPath = assemblyInfo["Folder"].Value + assemblyPartName;

            Logger.Info("Saving Assembly as '.SLDPRT'...");
            Logger.Info("More Specifically: " + assemblyPartPath);

            swApp.ActivateDoc3((string)assemblyInfo["Title"].Value, true, (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);

            check = modelExt.SaveAs(assemblyPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, errors, warnings);

            if (errors != 0) { Logger.Error("VolumeComparator", "SaveAsPart", "swSaveAsError: " + errors); }
            else if (warnings != 0) { Logger.Warn("swFileSaveWarning: " + warnings); }
            else if (check)
            {
                Logger.Info("Save successful.");
            }
            else { Logger.Error("VolumeComparator", "SaveAsPart", "Couldn't save for unknown reason."); }

            CloseDocs(new object[] { assembly });

            convertedAssembly = SwApp.OpenFile(assemblyPartPath);

            FeatureManager partFeatureMgr = convertedAssembly.FeatureManager;
            Feature partFeature;

            bodies_array = ((PartDoc)convertedAssembly).GetBodies2((int)swBodyType_e.swSolidBody, true);

            Body2[] bodies = new Body2[bodies_array.Length];

            for (int i = 0; i < bodies_array.Length; i++)
            {
                bodies[i] = (Body2)bodies_array[i];
            }

            Logger.Info("Combining all bodies together...");
            partFeature = partFeatureMgr.InsertCombineFeature((int)swBodyOperationType_e.SWBODYADD, null, bodies);
            if (partFeature == null)
            {
                Logger.Warn("No Body found to be touching eachother");
                return null;
            }

            convertedAssembly.Save();

            return convertedAssembly;
        }

        static void CloseDocs(object[] docs)
        {
            foreach (object doc in docs)
            {
                try
                {
                    ModelDoc2 modelDoc = (ModelDoc2)doc;
                    swApp.CloseDoc(modelDoc.GetTitle());
                }
                catch
                {
                    Logger.Error("VolumeComparator.cs", "CloseDocs()", "One of the provided documents cannot be converted to ModelDoc2");
                }
            }
        }

        static void CreateConfigurations(ModelDoc2 comparePart, string component1Name, string component2Name)
        {
            Logger.Info("Creating three (3) configurations...");
            config1 = comparePart.AddConfiguration3(component1Name + "_minus_"+ component2Name, 
                "Substraction of body1 minus body2", "", (int)swConfigurationOptions2_e.swConfigOption_DontActivate);
            config2 = comparePart.AddConfiguration3(component2Name + "_minus_" + component1Name,
                "Substraction of body2 minus body1", "", (int)swConfigurationOptions2_e.swConfigOption_DontActivate);
            config3 = comparePart.AddConfiguration3(component1Name + "_and_" + component2Name,
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

        static double SubstractVolume(ModelDoc2 comparePart, Body2 a, Body2 b)
        {
            FeatureManager partFeatureMgr = comparePart.FeatureManager;
            Feature partFeature;
            object[] localBodies;
            double totalBodyVolume = 0;
            Body2[] bodies_array = new Body2[] { b };
            double[] bodyproperties;
            bool suppression;
            double a_volume = (a.GetMassProperties(0))[3];
            double b_volume = (b.GetMassProperties(0))[3];

            Logger.Info("Inserting a Combine Feature: Substraction...");
            partFeature = partFeatureMgr.InsertCombineFeature((int)swBodyOperationType_e.SWBODYCUT, a, bodies_array);
            if (partFeature == null)
            {
                if (b_volume >= a_volume)
                {
                    Logger.Warn("Body B seems to encompass all of Body A yielding a null volume.");
                    return 0;
                }
                else
                {
                    Logger.Error("VolumeComparator", "SubstractVolume", "Could not create Feature");
                }
            }
            Logger.Info("New Combine Feature added.");

            localBodies = ((PartDoc)comparePart).GetBodies2((int)swBodyType_e.swSolidBody, true);

            foreach (Body2 body in localBodies)
            {
                bodyproperties = body.GetMassProperties(0);
                totalBodyVolume += bodyproperties[3];
            }

            suppression = partFeature.SetSuppression2((int)swFeatureSuppressionAction_e.swSuppressFeature, (int)swInConfigurationOpts_e.swThisConfiguration, null);

            if (suppression == true)
            {
                comparePart.Save();
                return totalBodyVolume;  // returns the yielded volume
            }
            else
            {
                Logger.Error("VolumeComparator.cs", "SubstractVolume()", "Could not suppress feature");
                return -1; // Suppression did not work
            }
                
        }

        static double CommonVolume(ModelDoc2 comparePart, Body2 a, Body2 b)
        {
            FeatureManager partFeatureMgr = comparePart.FeatureManager;
            Feature partFeature;
            object[] localBodies;
            double totalBodyVolume = 0;
            Body2[] bodies_array = new Body2[] { a, b };
            double[] bodyproperties;
            bool suppression;
            double a_volume = (a.GetMassProperties(0))[3];
            double b_volume = (b.GetMassProperties(0))[3];

            Logger.Info("Inserting a Combine Feature: Intersection...");
            partFeature = partFeatureMgr.InsertCombineFeature((int)swBodyOperationType_e.SWBODYINTERSECT, null, bodies_array);
            if (partFeature == null)
            {
                Logger.Warn("No Intersection found between the provided bodies");
                return 0;  // Zero Volume
            }
            Logger.Info("New Combine Feature added.");

            localBodies = ((PartDoc)comparePart).GetBodies2((int)swBodyType_e.swSolidBody, true);

            foreach (Body2 body in localBodies)
            {
                bodyproperties = body.GetMassProperties(0);
                totalBodyVolume += bodyproperties[3];
            }

            suppression = partFeature.SetSuppression2((int)swFeatureSuppressionAction_e.swSuppressFeature, (int)swInConfigurationOpts_e.swThisConfiguration, null);

            if (suppression == true)
            {
                comparePart.Save();
                return totalBodyVolume;  // returns the yielded volume
            }
            else
            {
                Logger.Error("VolumeComparator.cs", "SubstractVolume()", "Could not suppress feature");
                return -1;  // Suppression did not work
            }
        }

        static int CompareVolume(Body2 a, Body2 b)
        {
            /* returns 0 for no change in volume
             * returns 1 for negative change
             * returns 2 for positive change */
            double[] body1MassProp, body2MassProp;
            double body1Volume, body2Volume;

            body1MassProp = a.GetMassProperties(0);  // Using a density of '0' since we don't need mass
            body2MassProp = b.GetMassProperties(0);

            body1Volume = Math.Round(body1MassProp[3], 8);
            body2Volume = Math.Round(body2MassProp[3], 8);

            Logger.Info("Volume A:" + body1Volume + "\tVolume B:" + body2Volume);

            return IncDecDoubleReport(body1Volume, body2Volume, "CompareVolume()");
        }

        static int CompareFaces(Body2 a, Body2 b)
        {
            /* returns 0 for no change in number of faces
             * returns 1 for negative change
             * returns 2 for positive change */

            int body1Faces, body2Faces;

            body1Faces = a.GetFaceCount();
            body2Faces = b.GetFaceCount();


            Logger.Info("Faces A:" + body1Faces + "\tFaces B:" + body2Faces);

            return IncDecIntReport(body1Faces, body2Faces, "CompareFaces()");
        }

        static int CompareArea(Body2 a, Body2 b)
        {
            /* returns 0 for no change in total surface
             * returns 1 for negative change
             * returns 2 for positive change */

            object[] body1Faces, body2Faces;
            double body1Area, body2Area;


            body1Faces = a.GetFaces();
            body2Faces = b.GetFaces();

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

        static int casefinder(double volumeA, double volumeB, int volumedifference, int facedifference, int areadifference, double amb, double bma, double anb)
        {
            if (volumedifference == 0 && areadifference == 0 && amb > 0 && bma > 0 && anb > 0)
            {
                // Moved Feature
                return 1;
            }
            else if (volumedifference == 2 && areadifference == 2 && (facedifference == 0 || facedifference == 2) && amb == 0 && bma > 0 && anb == volumeA)
            {
                // New Extrusion
                return 2;
            }
            else if (volumedifference == 1 && areadifference == 1 && (facedifference == 0 || facedifference == 1) && amb > 0 && bma == 0 && anb == volumeB)
            {
                // Removed Extrusion
                return 3;
            }
            else if (volumedifference == 1 && facedifference == 2 && amb > 0 && bma == 0 && anb == volumeB)
            {
                // New Hole
                return 4;
            }
            else if (volumedifference == 2 && facedifference == 1 && amb == 0 && bma > 0 && anb == volumeA)
            {
                // Removed Hole
                return 5;
            }
            else if (facedifference == 2 && amb > 0 && bma > 0 && anb != volumeA && anb != volumeB)
            {
                // New Hole and Extrusion
                return 6;
            }
            else if (facedifference == 1 && amb > 0 && bma > 0 && anb != volumeA && anb != volumeB)
            {
                // Removed hole and extrusion
                return 7;
            }
            else if (volumedifference == 0 && areadifference == 0 && facedifference == 0  && amb > 0 && bma > 0 && anb == volumeA && anb == volumeB)
            {
                // Parts are the same
                return 8;
            }
            else
            {
                // Unknown Case
                Logger.Warn("Unknown Case:" +
                    "\n\t\t\t\t\t\t\t\t Volume A:     " + volumeA +
                    "\n\t\t\t\t\t\t\t\t Volume B:     " + volumeB +
                    "\n\t\t\t\t\t\t\t\t Volume Diff.: " + volumedifference +
                    "\n\t\t\t\t\t\t\t\t Face # Diff.: " + facedifference +
                    "\n\t\t\t\t\t\t\t\t Area # Diff.: " + areadifference +
                    "\n\t\t\t\t\t\t\t\t Volume A-B:   " + amb +
                    "\n\t\t\t\t\t\t\t\t Volume B-A:   " + bma +
                    "\n\t\t\t\t\t\t\t\t Volume A&B:   " + anb);
                return -1;
            }
        }
    }
}
