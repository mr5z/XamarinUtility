using System;

namespace XamarinUtility.Attributes
{
    public class RequiresRoleAttribute : Attribute
    {
        public RequiresRoleAttribute(string roles)
        {
            Roles = roles;
        }

        public string Roles { get; }
    }
}
