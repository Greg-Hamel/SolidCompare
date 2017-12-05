using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using SldWorks;
using SwConst;
using SWUtilities;

namespace SolidCompare
{
    class SwApp
    {
        private static SldWorks.SldWorks instance;
        private static gtcocswUtilities swUtil;
        static string programFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
        private static string swPath = programFile + @"\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe";  // Change this to your SolidWorks Directory

        public static SldWorks.SldWorks Instance
        {
            get
            {
                if (instance == null)
                {
                    Logger.Info("Handshaking with SolidWorks...");
                    instance = new SldWorks.SldWorks();
                    Logger.Info("SolidWorks handshake completed");
                }
                return instance;
            }
        }

        public static void Dispose()
        {
            Logger.Info("Closing SolidWorks...");
            instance.ExitApp();
            Logger.Info("SolidWorks Closed");
        }

        public static void LoadSwUtilities()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (instance != null)
            {
                if (swUtil == null)
                {
                    Logger.Info("Loading SolidWorks utilities...");
                    int status = instance.LoadAddIn(instance.GetExecutablePath() + "\\sldUtils\\SwLoaderSw.dll");

                    if (status == (int)swLoadAddinError_e.swSuccess)
                    {
                        // Confirm by loading again
                        status = instance.LoadAddIn(instance.GetExecutablePath() + "\\sldUtils\\SwLoaderSw.dll");
                    }

                    if (status == (int)swLoadAddinError_e.swSuccess || status == (int)swLoadAddinError_e.swAddinAlreadyLoaded)
                    {
                        swUtil = (gtcocswUtilities)instance.GetAddInObject("Utilities.UtilitiesApp");

                        if (swUtil == null)
                        {
                            Logger.Warn("Utilities loaded but failed to get gtcocswUtilities object");
                        }
                    }
                    else
                    {
                        Logger.Error(typeof(SwApp).FullName, methodName, "Failed to load utilities. swLoadAddinError_e: " + status);
                    }
                }
            }
            else
            {
                Logger.Error(typeof(SwApp).FullName, methodName, "SolidWorks instance not found");
            }
        }

        public static object GetSwToolInterface(gtSwTools_e toolName)
        {
            object toolObject = null;
            int errorCode = 0;

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            if (null != swUtil)
            {
                toolObject = swUtil.GetToolInterface((int)toolName, out errorCode);
            }
            if ((int)gtError_e.gtNOErr != errorCode)
            {
                Logger.Error(typeof(SwApp).FullName, methodName, "Failed to get tool " + toolName + ". gtError_e: " + errorCode);
            }
            return toolObject;
        }

        public static gtcocswCompareGeometry GetSwCompareGeometry()
        {
            return (gtcocswCompareGeometry)GetSwToolInterface(gtSwTools_e.gtSwToolGeomDiff);
        }

        public static ModelDoc2 OpenFile(string fileName)
        {
            /* Returns the file's IPartDoc, IAssemblyDoc or
             * IModelDoc2 if it is a part, assembly or 
             * anything else that can be loaded. */

            if (fileName == null)
            {
                Logger.Error("SwApp.cs", "OpenFile()", "The filename provided is null");
            }

            ModelDoc2 swModel = default(ModelDoc2);
            IDocumentSpecification swDocSpecification = default(DocumentSpecification);
            int errors = 0;
            int warnings = 0;
            string nameOfFile;
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            int index = fileName.LastIndexOf("\\");
            nameOfFile = fileName.Substring(index + 1);

            string activeFolder = fileName.Substring(0, index);
            instance.SetCurrentWorkingDirectory(activeFolder);


            // Set the specifications
            swDocSpecification = (IDocumentSpecification)instance.GetOpenDocSpec(fileName);
            Logger.Info("'" + nameOfFile + "' is a " + (swDocumentTypes_e)swDocSpecification.DocumentType);
            swDocSpecification.Selective = false;
            //swDocSpecification.DocumentType = (int)swDocumentTypes_e.swDocASSEMBLY;
            // swDocSpecification.DisplayState = "Default_Display State-1";
            swDocSpecification.UseLightWeightDefault = false;
            swDocSpecification.LightWeight = false;
            swDocSpecification.Silent = true;
            swDocSpecification.IgnoreHiddenComponents = true;

            // Open the assembly document as per the specifications
            Logger.Info("Opening '" + nameOfFile + "'...");
            swModel = instance.OpenDoc7(swDocSpecification);
            errors = swDocSpecification.Error;
            warnings = swDocSpecification.Warning;

            if (errors != 0)
            {
                // See swFileLoadError_e for error codes
                Logger.Error(typeof(SwApp).FullName, methodName, "Failed to open File. error: " + errors);
            }
            if (warnings != 0)
            {
                // See swFileLoadWarning_e for warning codes
                if (warnings == 128)
                {
                    Logger.Info("The file is already open.");
                }
                else
                {
                    Logger.Warn("Opening File warning: " + warnings);
                }
            }
            return swModel;  // Returns the IModelDoc2
        }

        public static IModelDoc2 ActivateDocument(IModelDoc2 swModelDoc)
        {
            int errors = 0;
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            IModelDoc2 swActivatedModel = null;

            if (0 == errors)
            {
                swActivatedModel = instance.ActivateDoc3(swModelDoc.GetPathName(), false, (int)swRebuildOnActivation_e.swDontRebuildActiveDoc, ref errors);
            }
            else
            {
                Logger.Error(typeof(SwApp).FullName, methodName, "Failed to open Assembly. error: " + errors);
            }

            return swActivatedModel;
        }

        public static string GetLang()
        {
            string lang;

            lang = instance.GetCurrentLanguage();

            return lang;
        }

        public static void Cleanup(List<ModelDoc2> previousDocs)
        {
            List<ModelDoc2> currentDocs = ListCurrentlyOpened();
            List<ModelDoc2> toBeClosed = new List<ModelDoc2>();
            foreach (ModelDoc2 Doc in currentDocs)
            {
                if (!previousDocs.Contains(Doc))
                {
                    toBeClosed.Add(Doc);
                }
                else { /* pass */ }
            }
            Logger.Info(currentDocs.Count + " documents open.");
            Logger.Info(toBeClosed.Count + " documents to be closed.");
            CloseDocs(toBeClosed);
            Logger.Info(ListCurrentlyOpened().Count + " documents left open.");

            if (ListCurrentlyOpened().Count != (currentDocs.Count - toBeClosed.Count))
            {
                Logger.Warn("Not all documents were closed as expected...");
            }
            else { Logger.Info("Clean-Up completed successfully."); }

        }

        public static List<ModelDoc2> ListCurrentlyOpened()
        {
            List<ModelDoc2> openedDocs = new List<ModelDoc2>();

            ModelDoc2 swModel = Instance.GetFirstDocument();
            while (swModel != null)
            {
                /* Assembly components are opened, but are not visible
                 * until opened by the user */
                // Debug.WriteLine(swModel.GetTitle() + " [" + swModel.GetType() + "]");

                /* The document name contains a filename extension
                * if the document has been saved
                * and is subject to Microsoft Windows Explorer setting;
                * the document name does not contain a
                * filename extension if the document has not been saved;
                * ModelDoc2::GetPathName is blank until the file is saved */

                openedDocs.Add(swModel);
                swModel = swModel.GetNext();
            }

            return openedDocs;
        }

        public static void CloseDocs(List<ModelDoc2> docs)
        {
            foreach (ModelDoc2 doc in docs)
            {
                try
                {
                    Instance.CloseDoc(doc.GetTitle());
                }
                catch
                {
                    Logger.Error("SwApp.cs", "CloseDocs()", "One of the provided documents cannot be converted to ModelDoc2");
                }
            }
        }
    }
}
