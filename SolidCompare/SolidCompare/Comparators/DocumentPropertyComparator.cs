using System;
using SldWorks;
using SwConst;

namespace SolidCompare.Comparators
{
    class DocumentPropertyComparator : IComparator<IModelDoc2>
    {
        private static DocumentPropertyComparator instance;

        private DocumentPropertyComparator()
        {

        }

        public static DocumentPropertyComparator Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new DocumentPropertyComparator();
                }
                return instance;
            }
        }

        private Boolean IsSameSingleInfo(IModelDoc2 refDocument, IModelDoc2 modDocument, swSummInfoField_e type)
        {
            return (refDocument.SummaryInfo[(int)type] == modDocument.SummaryInfo[(int)type]);
        }

        public CompareResult Compare(IModelDoc2 refDocument, IModelDoc2 modDocument)
        {
            Boolean result = IsSameSingleInfo(refDocument, modDocument, swSummInfoField_e.swSumInfoAuthor)
                && IsSameSingleInfo(refDocument, modDocument, swSummInfoField_e.swSumInfoCreateDate)
                && IsSameSingleInfo(refDocument, modDocument, swSummInfoField_e.swSumInfoSaveDate)
                && IsSameSingleInfo(refDocument, modDocument, swSummInfoField_e.swSumInfoSavedBy);

            return new CompareResult(result ? CompareResultStatus.Identical : CompareResultStatus.Similar);
        }
    }
}
