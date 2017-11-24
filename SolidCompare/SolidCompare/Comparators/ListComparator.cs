using System;
using System.Collections.Generic;
using SolidCompare.Entities;

namespace SolidCompare.Comparators
{
    public class ListComparator : IComparator<List<AbstractEntity>>
    {
        private static ListComparator instance = null;

        private ListComparator()
        {

        }

        public static ListComparator Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new ListComparator();
                }
                return instance;
            }
        }

        public CompareResult Compare(List<AbstractEntity> reference, List<AbstractEntity> modified)
        {
            List<AbstractEntity> removed = new List<AbstractEntity>(reference);
            List<AbstractEntity> added = new List<AbstractEntity>(modified);
            List<CompareResult> resultModified = new List<CompareResult>();
            CompareResultStatus status = CompareResultStatus.NotPerformed;

            foreach (AbstractEntity referenceEntity in reference)
            {
                AbstractEntity modifiedEntity = modified.Find(x => x.GetID() == referenceEntity.GetID());

                if (modifiedEntity != null)
                {
                    resultModified.Add(referenceEntity.CompareTo(modifiedEntity));
                    removed.Remove(referenceEntity);
                    added.Remove(modifiedEntity);
                }
            }

            return new CompareResult(added, removed, resultModified, status);
        }
    }
}
