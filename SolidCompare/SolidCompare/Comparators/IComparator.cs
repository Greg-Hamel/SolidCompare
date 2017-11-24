
namespace SolidCompare.Comparators
{
    interface IComparator<T>
    {
        CompareResult Compare(T obj1, T obj2);
    }
}