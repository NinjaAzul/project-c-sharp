using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Project_C_Sharp.Shared.Errors;
using Project_C_Sharp.Shared.I18n.Modules.Auth.Errors.Keys;
using Project_C_Sharp.Shared.Resources.Users;

namespace Project_C_Sharp.Shared.Guards.AuthGuard.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthGuardAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorization))
        {
            SetUnauthorizedResult(context);
            return;
        }

        try
        {
            var token = authorization.Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            context.HttpContext.Items["UserId"] = userId;
        }
        catch
        {
            SetUnauthorizedResult(context);
        }
    }

    private static void SetUnauthorizedResult(AuthorizationFilterContext context)
    {
        var error = new ApiErrorResponse
        {
            TraceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
            Message = AuthResource.GetError(AuthErrorsKeys.Token_Invalid),
            StatusCode = StatusCodes.Status401Unauthorized,
        };

        context.Result = new UnauthorizedObjectResult(error);
    }
}