using System;
using SldWorks;
using SolidCompare.Comparators;

namespace SolidCompare.Entities
{
    public abstract class AbstractEntity : IComparableEntity<AbstractEntity>
    {
        protected IFeature relatedFeature = null;

        public AbstractEntity(IFeature relatedFeature)
        {
            this.relatedFeature = relatedFeature;
        }

        public virtual IFeature GetRelatedFeature()
        {
            return relatedFeature;
        }

        public virtual int GetID()
        {
            return relatedFeature?.GetID() ?? 0;
        }

        public virtual string GetName()
        {
            return relatedFeature?.Name ?? "<Root>";
        }

        public override string ToString()
        {
            return "[" + GetID() + "] " + GetName();
        }

        public abstract CompareResult CompareTo(AbstractEntity target);
    }
}
