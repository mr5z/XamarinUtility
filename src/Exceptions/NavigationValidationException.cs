using System;
using XamarinUtility.Extensions.Navigations;

namespace XamarinUtility.Exceptions
{
    class NavigationValidationException : Exception
    {
        public NavigationValidationException(ViewModelMetadata pageInfo, string? message = null) : base(message)
        {
            PageInfo = pageInfo;
        }

        public ViewModelMetadata PageInfo { get; }
    }
}
