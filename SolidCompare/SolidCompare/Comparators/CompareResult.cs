using SolidCompare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCompare.Comparators
{
    public enum CompareResultStatus
    {
        NotPerformed = -1,
        Identical = 0,
        Similar = 1,
        Different = 2,
    }

    public enum VolumeCompareResultStatus
    {
        Unknown = -1,
        MovedFeature = 1,
        NewExtrusion = 2,
        RemovedExtrusion = 3,
        NewHole = 4,
        RemovedHole = 5,
        NewHoldAndExtrusion= 6,
        RemovedHoleAndExtrusion = 7,
        Identical = 8,
    }

    public class CompareResult
    {
        private int comparedId;
        private List<Object> added = new List<object>();
        private List<Object> removed = new List<object>();
        private List<Object> changed = new List<object>();
        private List<Object> unchanged = new List<object>();

        private CompareResultStatus status = CompareResultStatus.NotPerformed;

        public CompareResult()
        {

        }

        public CompareResult(int comparedId, CompareResultStatus status)
        {
            this.comparedId = comparedId;
            this.status = status;
        }

        public CompareResult(CompareResultStatus status)
        {
            this.status = status;
        }

        public CompareResult(List<Object> added, List<Object> removed, List<Object> changed, List<Object> unchanged, CompareResultStatus status)
        {
            this.added = added;
            this.removed = removed;
            this.changed = changed;
            this.unchanged = unchanged;
            this.status = status;
        }

        public int ComparedId
        {
            get
            {
                return comparedId;
            }
            set
            {
                comparedId = value;
            }
        }

        public List<Object> Unchanged
        {
            get
            {
                return unchanged;
            }
            set
            {
                unchanged = value;
            }
        }

        public List<Object> Added
        {
            get
            {
                return added;
            }
            set
            {
                added = value;
            }
        }

        public List<Object> Removed
        {
            get
            {
                return removed;
            }
            set
            {
                removed = value;
            }
        }

        public List<Object> Changed
        {
            get
            {
                return changed;
            }
            set
            {
                changed = value;
            }
        }

        public CompareResultStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public CompareResult Merge(CompareResult newResult)
        {
            added.AddRange(newResult.Added);
            changed.AddRange(newResult.Changed);
            removed.AddRange(newResult.Removed);
            unchanged.AddRange(newResult.Unchanged);
            status = (CompareResultStatus)Math.Max((int)status, (int)newResult.Status);

            return this;
        }

        private string ListToString(string listName, List<Object> list)
        {
            string result = list.Count > 0 ? "\t" + listName + "\n" : "";

            foreach (Object element in list)
            {
                result += "\t\t " + element + "\n";
            }

            return result;
        }

        public override string ToString()
        {
            string result = "[COMPARING: id = " + comparedId + "] " + Status + "\n";
            result += ListToString("[ADDED]", added) + ListToString("[REMOVED]", removed)
                + ListToString("[CHANGED]", changed) + ListToString("[UNCHANGED]", unchanged);

            return result;
        }
    }
}
