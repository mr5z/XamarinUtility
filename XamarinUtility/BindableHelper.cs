using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace XamarinUtility
{
    public static class BindableHelper
    {
        private const string KnownPropertyPattern = "Property";

        public static BindableProperty CreateProperty<T>(
            T defaultValue = default,
            BindingMode mode = BindingMode.TwoWay,
            BindableProperty.BindingPropertyChangedDelegate propertyChanged = null,
            [CallerMemberName]string propertyName = null)
        {
            propertyName = RemoveLastOccurrence(propertyName, KnownPropertyPattern);
            return BindableProperty.Create(
                propertyName,
                typeof(T),
                typeof(BindableObject),
                defaultValue,
                mode,
                propertyChanged: propertyChanged);
        }

        public static BindablePropertyKey CreateReadonlyProperty<T>(
            T defaultValue = default,
            BindingMode mode = BindingMode.TwoWay,
            BindableProperty.BindingPropertyChangedDelegate propertyChanged = null,
            [CallerMemberName] string propertyName = null)
        {
            propertyName = RemoveLastOccurrence(propertyName, KnownPropertyPattern);
            return BindableProperty.CreateReadOnly(
                propertyName,
                typeof(T),
                typeof(BindableObject),
                defaultValue,
                mode,
                propertyChanged: propertyChanged);
        }

        public static BindableProperty CreateAttached<T>(
            T defaultValue = default,
            BindingMode mode = BindingMode.TwoWay,
            BindableProperty.BindingPropertyChangedDelegate propertyChanged = null,
            [CallerMemberName] string propertyName = null)
        {
            propertyName = RemoveLastOccurrence(propertyName, KnownPropertyPattern);
            return BindableProperty.CreateAttached(
                propertyName,
                typeof(T),
                typeof(BindableObject),
                defaultValue,
                mode,
                propertyChanged: propertyChanged);
        }

        private static string RemoveLastOccurrence(string source, string toFind)
        {
            var index = source.LastIndexOf(toFind);
            return index == -1 ? source : source.Substring(0, index);
        }
    }
}
