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
        Equal = 0,
        Similar = 1,
        Different = 2,
    }

    public class CompareResult
    {
        Object added;
        Object removed;
        Object modified;
        CompareResultStatus status;

        public CompareResult()
        {

        }

        public CompareResult(CompareResultStatus status)
        {
            this.status = status;
        }

        public CompareResult(object added, object removed, object modified, CompareResultStatus status)
        {
            this.added = added;
            this.removed = removed;
            this.modified = modified;
            this.status = status;
        }

        public Object Added
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

        public Object Removed
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

        public Object Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
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
    }
}
