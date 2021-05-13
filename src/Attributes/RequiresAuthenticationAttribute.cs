using System;

namespace XamarinUtility.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiresAuthenticationAttribute : Attribute
    {
        public RequiresAuthenticationAttribute(bool isRequired = true)
        {
            IsRequired = isRequired;
        }

        public bool IsRequired { get; }
    }
}
