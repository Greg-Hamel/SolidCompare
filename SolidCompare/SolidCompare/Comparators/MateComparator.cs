using SolidCompare.Entities;

namespace SolidCompare.Comparators
{
    class MateComparator : IComparator<Mate>
    {
        private static MateComparator instance = null;

        private MateComparator()
        {

        }

        public static MateComparator Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new MateComparator();
                }
                return instance;
            }
        }

        public CompareResult Compare(Mate obj1, Mate obj2)
        {
            throw new System.NotImplementedException();
        }
    }
}
