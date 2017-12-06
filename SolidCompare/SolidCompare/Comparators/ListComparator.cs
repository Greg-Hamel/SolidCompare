using System;
using System.Linq;
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

        public CompareResult Compare(List<AbstractEntity> referenceList, List<AbstractEntity> modifiedList)
        {
            List<Object> removed = new List<AbstractEntity>(referenceList).Cast<Object>().ToList();
            List<Object> added = new List<AbstractEntity>(modifiedList).Cast<Object>().ToList();
            List<Object> changed = new List<CompareResult>().Cast<Object>().ToList();
            List<Object> unchanged = new List<CompareResult>().Cast<Object>().ToList();
            CompareResultStatus status = CompareResultStatus.NotPerformed;

            foreach (AbstractEntity referenceEntityToCompare in referenceList)
            {

                AbstractEntity modifiedEntityToCompare = modifiedList.Find(x => x.GetID() == referenceEntityToCompare.GetID());

                if (modifiedEntityToCompare != null)
                {
                    CompareResult result = referenceEntityToCompare.CompareTo(modifiedEntityToCompare);

                    if (CompareResultStatus.Different == result.Status || CompareResultStatus.Similar == result.Status)
                    {
                        changed.Add(referenceEntityToCompare);                        
                        // changed.Add(result);
                    }
                    else
                    {
                        unchanged.Add(referenceEntityToCompare);                        
                    }

                    removed.Remove(referenceEntityToCompare);
                    added.Remove(modifiedEntityToCompare);

                    status = (CompareResultStatus)Math.Max((int)status, (int)result.Status);
                }
            }

            if (added.Count > 0 || removed.Count > 0)
            {
                status = CompareResultStatus.Different;
            }

            return new CompareResult(added, removed, changed, unchanged, status);
        }
    }
}
