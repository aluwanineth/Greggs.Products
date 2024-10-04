using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Greggs.Products.Application.Wrappers;

namespace Greggs.Products.Api.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

            switch (error)
            {
                case Application.Exceptions.ApiException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError($"Stack Trace: {e.StackTrace} Inner Exception {e.InnerException} Message:{e.Message}");
                    break;
                case KeyNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogError($"Stack Trace: {e.StackTrace} Inner Exception {e.InnerException} Message:{e.Message}");
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(error, "An unhandled exception occurred: {Message}", error.Message);
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel);

            await response.WriteAsync(result);
        }
    }
}
