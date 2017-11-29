using System;
using SldWorks;
using SWUtilities;

namespace SolidCompare.Comparators
{
    class GeometryComparator : IComparator<IModelDoc2>
    {
        private static GeometryComparator instance = null;

        private GeometryComparator()
        {
            SwApp.LoadSwUtilities();
        }

        public static GeometryComparator Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new GeometryComparator();
                }
                return instance;
            }
        }

        public CompareResult Compare(IModelDoc2 refDocument, IModelDoc2 modDocument)
        {
            string refFileName = refDocument.GetPathName();
            string modFileName = modDocument.GetPathName();
            int volumeDiffStatus = 0;
            int faceDiffStatus = 0;

            SwApp.GetSwCompareGeometry().CompareGeometry3(refFileName, "", modFileName, "",
                (int)gtGdfOperationOption_e.gtGdfFaceAndVolumeCompare, (int)gtResultOptions_e.gtResultNoUI, "",
                false, false, ref volumeDiffStatus, ref faceDiffStatus);

            // Console.WriteLine(" +++++ GeomDiff " + volumeDiffStatus + " --- " + faceDiffStatus + "   " + refFileName + "  " + modFileName);

            return new CompareResult();
        }
    }
}
