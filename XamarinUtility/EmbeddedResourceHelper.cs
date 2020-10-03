using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace XamarinUtility
{
    public static class EmbeddedResourceHelper
    {
        //public static Stream FromResources(string resourceFileName)
        //{
        //    var assembly = typeof(App).GetTypeInfo().Assembly;
        //    var resourceNames = assembly.GetManifestResourceNames();
        //    resourceFileName = "UNGlobalGoals.Resources." + resourceFileName;
        //    var resourcePaths = resourceNames
        //        .Where(x => x.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase))
        //        .ToArray();

        //    if (!resourcePaths.Any())
        //    {
        //        throw new Exception(string.Format("Resource ending with {0} not found.", resourceFileName));
        //    }

        //    if (resourcePaths.Count() > 1)
        //    {
        //        throw new Exception(string.Format("Multiple resources ending with {0} found: {1}{2}", resourceFileName, Environment.NewLine, string.Join(Environment.NewLine, resourcePaths)));
        //    }

        //    return assembly.GetManifestResourceStream(resourcePaths.Single());
        //}
    }
}
