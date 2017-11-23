using SolidCompare.Entities;

namespace SolidCompare.Comparators
{
    class BodyFeatureComparator : IComparator<BodyFeature>
    {
        private static BodyFeatureComparator instance = null;

        private BodyFeatureComparator()
        {

        }

        public static BodyFeatureComparator Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new BodyFeatureComparator();
                }
                return instance;
            }
        }
        
        public CompareResult Compare(BodyFeature feature1, BodyFeature feature2)
        {
            throw new System.NotImplementedException();
        }
    }
}
