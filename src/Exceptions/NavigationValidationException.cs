using System;
using XamarinUtility.Extensions.Navigations;

namespace XamarinUtility.Exceptions;

class NavigationValidationException(ViewModelMetadata pageInfo, string? message = null) : Exception(message)
{
    public ViewModelMetadata PageInfo { get; } = pageInfo;
}
