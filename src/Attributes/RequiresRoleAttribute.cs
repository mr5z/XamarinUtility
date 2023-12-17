using System;

namespace XamarinUtility.Attributes;

public class RequiresRoleAttribute(string roles) : Attribute
{
    public string Roles { get; } = roles;
}
