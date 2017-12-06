using System;
using SolidCompare.Comparators;

namespace SolidCompare.Entities
{
    public interface IComparableEntity<T>
    {
        int GetID();
        string GetName();
        
        CompareResult CompareTo(T target);
    }
}
