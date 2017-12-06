using System;
using SldWorks;
using SolidCompare.Comparators;

namespace SolidCompare.Entities
{
    public class BodyFeature : AbstractEntity
    {
        public BodyFeature(IFeature swFeature) : base(swFeature)
        {

        }

        public override CompareResult CompareTo(AbstractEntity target)
        {
            IFeature refFeature = relatedFeature;
            IFeature modFeature = target.GetRelatedFeature();

            CompareResult result = FeaturePropertyComparator.Instance.Compare(refFeature, modFeature);

            // Deep comparison not implemented yet for BodyFeature

            return new CompareResult(GetID(), result.Status) {
                Details = "Possible undetectable changes directly on the assembly through this feature; please check manually"
            };
        }
    }
}
