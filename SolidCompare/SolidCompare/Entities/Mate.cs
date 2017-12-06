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

        public IMate2 SwMate
        {
            get
            {
                return swMate;
            }
        }

        public override CompareResult CompareTo(AbstractEntity target)
        {
            IFeature refFeature = relatedFeature;
            IFeature modFeature = target.GetRelatedFeature();
            CompareResult finalResult = new CompareResult(GetID(), CompareResultStatus.Identical);

            // Shallow comparison
            CompareResult shallowCompareResult = FeaturePropertyComparator.Instance.Compare(refFeature, modFeature);
            shallowCompareResult.ComparedId = GetID();

            if (CompareResultStatus.Identical != shallowCompareResult.Status)
            {
                shallowCompareResult.Details = "Possible undetectable changes have been made; please check the mate manually";

                // Deep Comparison
                CompareResult deepCompareResult = MateComparator.Instance.Compare(this, (Mate)target);

                finalResult = deepCompareResult.Status > shallowCompareResult.Status ? deepCompareResult : shallowCompareResult;
            }

            return finalResult;
        }
    }
}
