using Prism.Ioc;

namespace XamarinUtility.Helpers;

public static class DependencyLocator
{
    public static T Resolve<T>()
        => Prism.PrismApplicationBase.Current.Container.Resolve<T>();
}