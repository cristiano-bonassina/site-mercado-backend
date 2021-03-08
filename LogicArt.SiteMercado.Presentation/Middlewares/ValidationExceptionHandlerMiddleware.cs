using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LogicArt.Arch.Application.Validations.Abstractions.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public class ValidationExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        try
        {
            await next.Invoke(context);
        }
        catch (ValidationException exception)
        {
            var response = new { errors = exception.Result.Errors.Select(x => x.Error) };
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(response);
        }

    }

}

public static class ValidationExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ValidationExceptionHandlerMiddleware>();
        return app;
    }
}