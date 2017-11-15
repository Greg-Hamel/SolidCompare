using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCompare
{
    class AssemblyComparator : FeatureComparator
    {
        public AssemblyComparator(object referenceFeature, object modifiedFeature) : base(referenceFeature, modifiedFeature)
        {
        }
    }
}
