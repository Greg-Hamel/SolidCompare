using System;
using SldWorks;
using SolidCompare.Comparators;

namespace SolidCompare.Entities
{
    public class Part : AbstractEntity
    {
        private IPartDoc swPart = default(IPartDoc);

        public Part(IPartDoc part) : this(part, null)
        {

        }

        public Part(IPartDoc swPart, IFeature relatedFeature) : base(relatedFeature)
        {
            this.swPart = swPart;
        }

        public override CompareResult CompareTo(AbstractEntity target)
        {
            return new CompareResult(CompareResultStatus.NotPerformed);
        }

    }
}
