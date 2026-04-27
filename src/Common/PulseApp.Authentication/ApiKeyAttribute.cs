using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PulseApp.Domain.Options;

namespace PulseApp.Authentication;

/// <summary>
/// Атрибут доступа только по ключу
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IOptions<ApiKeyOption>>();

        string? requestKey = context.HttpContext.Request.Headers["x-api-key"].ToString();

        if (string.IsNullOrEmpty(requestKey) || requestKey != config.Value.Key)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
    }
}
