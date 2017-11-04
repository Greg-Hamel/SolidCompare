using System;
using SldWorks;
using SwConst;
using SWUtilities;

namespace SolidCompare
{
    class SwApp
    {
        private static SldWorks.SldWorks instance;
        private static gtcocswUtilities swUtil;

        public static SldWorks.SldWorks Instance
        {
            get
            {
                if (instance == null)
                {
                    Logger.Info("Loading SolidWorks...");
                    instance = new SldWorks.SldWorks();
                    Logger.Info("SolidWorks loaded");
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

        public static IAssemblyDoc OpenAssembly(string fileName)
        {
            IModelDoc2 swModel = default(ModelDoc2);
            IDocumentSpecification swDocSpecification = default(DocumentSpecification);
            int errors = 0;
            int warnings = 0;

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            // Set the specifications
            swDocSpecification = (IDocumentSpecification)instance.GetOpenDocSpec(fileName);
            swDocSpecification.Selective = false;
            swDocSpecification.DocumentType = (int)swDocumentTypes_e.swDocASSEMBLY;
            // swDocSpecification.DisplayState = "Default_Display State-1";
            swDocSpecification.UseLightWeightDefault = false;
            swDocSpecification.LightWeight = false;
            swDocSpecification.Silent = true;
            swDocSpecification.IgnoreHiddenComponents = true;

            // Open the assembly document as per the specifications
            swModel = instance.OpenDoc7(swDocSpecification);
            errors = swDocSpecification.Error;
            warnings = swDocSpecification.Warning;

            if (errors != 0)
            {
                // See swFileLoadError_e for error codes
                Logger.Error(typeof(SwApp).FullName, methodName, "Failed to open Assembly. error: " + errors);
            }
            if (warnings != 0)
            {
                // See swFileLoadWarning_e for warning codes
                Logger.Warn("Opening Assembly warning: " + warnings);
            }

            return (IAssemblyDoc)swModel;
        }
    }
}
