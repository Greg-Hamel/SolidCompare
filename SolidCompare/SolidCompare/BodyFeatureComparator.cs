using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCompare
{
    class BodyFeatureComparator : FeatureComparator
    {
        public BodyFeatureComparator(object referenceFeature, object modifiedFeature) : base(referenceFeature, modifiedFeature)
        {
        }
    }
}
