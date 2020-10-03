using System;
using System.Collections.Generic;
using ParameterizedFunc = System.Func<object, object>;

namespace XamarinUtility
{
    public class Mapper
    {
        private static Mapper instance;
        public static Mapper Instance => instance ??= new Mapper();

        private readonly IDictionary<(Type, Type), ParameterizedFunc> objectDictionary =
            new Dictionary<(Type, Type), ParameterizedFunc>();

        private Mapper() { }

        public void Register<TSource, TDestination>(Func<TSource, TDestination> mapping)
        {
            var key = ToKey<TSource, TDestination>();
            objectDictionary[key] = (arg) => mapping((TSource)arg);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            var key = ToKey<TSource, TDestination>();
#if DEBUG
            if (!objectDictionary.ContainsKey(key))
            {
                throw new ArgumentException($"Source of type '{typeof(TSource).Name}' and destination of type '{typeof(TDestination).Name}' is not registered yet.");
            }
#endif
            return (TDestination)objectDictionary[key](source);
        }

        private static (Type, Type) ToKey<TSource, TDestination>()
        {
            return (typeof(TSource), typeof(TDestination));
        }
    }
}
