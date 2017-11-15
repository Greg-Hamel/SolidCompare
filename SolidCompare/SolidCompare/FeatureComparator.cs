using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCompare
{
    class FeatureComparator
    {
        private object referenceFeature;
        private object modifiedFeature;

        public FeatureComparator(object referenceFeature, object modifiedFeature)
        {
            this.referenceFeature = referenceFeature;
            this.modifiedFeature = modifiedFeature;
        }

        public int Compare()
        {
            return 0;
        }
    }
}
