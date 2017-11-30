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
            ModelDoc2 refPartDoc = (ModelDoc2)SwPart;
            ModelDoc2 modPartDoc = (ModelDoc2)((Part)target).SwPart;
            
            CompareResult nomenclatureResult = DocumentPropertyComparator.Instance.Compare(refPartDoc, modPartDoc);
            // Console.WriteLine(" +++++ Part " + targetPartDoc.GetPathName() + " --- " + result);

            // Deep Comparison
            CompareResult geometricResult = new VolumeComparator().Compare(refPartDoc, modPartDoc);

            if (nomenclatureResult.Status == CompareResultStatus.Identical && geometricResult.Status == CompareResultStatus.Identical)
            {
                return new CompareResult(GetID(), CompareResultStatus.Identical);
            }
            else if (nomenclatureResult.Status == CompareResultStatus.Identical && geometricResult.Status == CompareResultStatus.Different)
            {
                return new CompareResult(GetID(), CompareResultStatus.Similar);
            }
            else if (nomenclatureResult.Status == CompareResultStatus.Similar && geometricResult.Status == CompareResultStatus.Identical)
            {
                return new CompareResult(GetID(), CompareResultStatus.Identical);
            }
            else if (nomenclatureResult.Status == CompareResultStatus.Similar && geometricResult.Status == CompareResultStatus.Different)
            {
                return new CompareResult(GetID(), CompareResultStatus.Different);
            }
            else
            {
                Logger.Warn("Parts Comparison has returned an unexpected value.");
                return new CompareResult(GetID(), CompareResultStatus.NotPerformed);
            }
        }

    }
}
