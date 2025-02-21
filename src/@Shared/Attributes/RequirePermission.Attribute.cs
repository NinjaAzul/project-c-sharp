using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Project_C_Sharp.Shared.Attributes.Permissions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _feature;
    private readonly string _permission;

    public RequirePermissionAttribute(string feature, string permission)
    {
        _feature = feature;
        _permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        context.HttpContext.Items["RequiredFeature"] = _feature;
        context.HttpContext.Items["RequiredPermission"] = _permission;
    }
}