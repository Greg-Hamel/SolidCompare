using System;
using SldWorks;

namespace SolidCompare.Comparators
{
    class FeaturePropertyComparator : IComparator<IFeature>
    {
        private static FeaturePropertyComparator instance;

        private FeaturePropertyComparator()
        {

        }

        public static FeaturePropertyComparator Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new FeaturePropertyComparator();
                }
                return instance;
            }
        }

        public CompareResult Compare(IFeature refFeature, IFeature modFeature)
        {
            Boolean result = refFeature.CreatedBy == modFeature.CreatedBy
                && refFeature.DateCreated == modFeature.DateCreated
                && refFeature.DateModified == modFeature.DateModified;

            return new CompareResult(result ? CompareResultStatus.Identical : CompareResultStatus.Similar);
        }
    }
}
