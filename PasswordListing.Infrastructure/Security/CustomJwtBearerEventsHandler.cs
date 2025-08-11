using System;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace PasswordListing.Infrastructure.Security;

public class CustomJwtBearerEventsHandler : JwtBearerEvents
{
    public override async Task Challenge(JwtBearerChallengeContext context)
    {
        context.HandleResponse();

        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Unauthorize Access, please insert valid credentials"
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    public override async Task Forbidden(ForbiddenContext context)
    {
        context.Response.StatusCode = 403;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Access denied. Doesnt access to resource"
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
