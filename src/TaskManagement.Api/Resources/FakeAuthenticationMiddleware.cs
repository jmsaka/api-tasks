using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.Api.Resources;

public class FakeAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public FakeAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        var authorizeAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

        if (authorizeAttribute != null)
        {
            var user = context.User;

            if (!string.IsNullOrEmpty(authorizeAttribute.Roles) && !user.IsInRole(authorizeAttribute.Roles))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync($"Acesso negado: Usuário não tem a role '{authorizeAttribute.Roles}'.");
                return;
            }
        }

        await _next(context);
    }
}
