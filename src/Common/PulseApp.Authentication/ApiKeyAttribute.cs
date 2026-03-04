using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PulseApp.Authentication;

/// <summary>
/// Атрибут доступа только по ключу
/// </summary>
public class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string validKey = "P6EEhl4VsmVgdHaej2vY";
        string? requestKey = context.HttpContext.Request.Headers["x-api-key"].ToString();

        if (string.IsNullOrEmpty(requestKey) || requestKey != validKey)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
    }
}
