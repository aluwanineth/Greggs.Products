using Greggs.Products.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Greggs.Products.Api.Extentions;

public static class AppExtensions
{
    public static void UseSwaggerExtension(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("../swagger/v1/swagger.json", "Greggs Products.Api");
        });
    }
    public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}
