using System;

namespace XamarinUtility.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequiresAuthenticationAttribute(bool isRequired = true) : Attribute
{
    public bool IsRequired { get; } = isRequired;
}
