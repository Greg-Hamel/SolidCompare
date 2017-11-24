using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
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
        private static string swPath =  programFile + @"\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe";  // Change this to your SolidWorks Directory

        public static SldWorks.SldWorks Instance
        {
            get
            {
                if (instance == null)
                {
                    bool starting = true;

                    try
                    {
                        Logger.Info("Looking for running process of SolidWorks...");
                        instance = (SldWorks.SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                        Logger.Info("Solidworks already running");
                    }
                    catch
                    {
                        Logger.Warn("Solidworks does not seem to be running, attempting to start SolidWorks...");
                        try { Process.Start(swPath); }
                        catch { Logger.Error("SwApp", "'Instance - Get'", "SolidWorks Path incorrect" + swPath); }
                        Logger.Info("Start command issued...");

                        do
                        {
                            Logger.Info("Waiting for Solidworks...");
                            try
                            {
                                instance = (SldWorks.SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                                starting = false;
                                Logger.Info("Solidworks has completed its startup sequence.");
                            }
                            catch
                            {
                                Logger.Info("Solidworks is still starting... Waiting 1 sec");
                                System.Threading.Thread.Sleep(1000);
                            }
                        } while (starting);
                    }
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
                    if ((int)swLoadAddinError_e.swSuccess == status || (int)swLoadAddinError_e.swAddinAlreadyLoaded == status)
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

            ModelDoc2 swModel = default(ModelDoc2);
            IDocumentSpecification swDocSpecification = default(DocumentSpecification);
            int errors = 0;
            int warnings = 0;
            string nameOfFile;

            int index = fileName.LastIndexOf("\\");
            nameOfFile = fileName.Substring(index+1);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            // Set the specifications
            swDocSpecification = (IDocumentSpecification)instance.GetOpenDocSpec(fileName);
            Logger.Info("'" + nameOfFile + "' is a " + (swDocumentTypes_e)swDocSpecification.DocumentType);
            swDocSpecification.Selective = false;
            // swDocSpecification.DocumentType = (int)swDocumentTypes_e.swDocASSEMBLY;
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
    }
}
