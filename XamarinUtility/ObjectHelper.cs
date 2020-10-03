using System.Collections;
using System.Collections.Generic;

namespace XamarinUtility
{
    public static class ObjectHelper
    {
        public static bool IsDefault<T>(T value) => EqualityComparer<T>.Default.Equals(value, default);
        public static bool IsNotDefault<T>(T value) => !IsDefault(value);
        public static bool IsEmptyList<T>(T value)
        {
            if (value is IList list)
                return list.Count <= 0;

            return false;
        }
    }
}
