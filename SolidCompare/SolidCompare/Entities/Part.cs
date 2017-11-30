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

        public IPartDoc SwPart
        {
            get
            {
                return swPart;
            }
        }

        public override CompareResult CompareTo(AbstractEntity target)
        {
            IModelDoc2 refPartDoc = (IModelDoc2)SwPart;
            IModelDoc2 modPartDoc = (IModelDoc2)((Part)target).SwPart;
            
            CompareResult result = DocumentPropertyComparator.Instance.Compare(refPartDoc, modPartDoc);
            // Console.WriteLine(" +++++ Part " + targetPartDoc.GetPathName() + " --- " + result);

            if (CompareResultStatus.Identical != result.Status)
            {
                // Deep Comparison
            }

            return new CompareResult(GetID(), result.Status);
        }

    }
}
