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
            return new CompareResult(CompareResultStatus.NotPerformed);
        }
    }
}
