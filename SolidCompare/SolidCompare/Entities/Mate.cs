using System;
using SldWorks;
using SolidCompare.Comparators;

namespace SolidCompare.Entities
{
    public class Mate : AbstractEntity
    {
        private IMate2 swMate = default(IMate2);

        public Mate(IFeature relatedFeature) : base(relatedFeature)
        {
            swMate = relatedFeature.GetSpecificFeature2();
        }

        public override CompareResult CompareTo(AbstractEntity target)
        {
            IFeature refFeature = relatedFeature;
            IFeature modFeature = target.GetRelatedFeature();

            CompareResult result = FeaturePropertyComparator.Instance.Compare(refFeature, modFeature);

            if (CompareResultStatus.Identical != result.Status)
            {
                // Deep Comparison
            }

            return new CompareResult(GetID(), result.Status);
        }
    }
}
