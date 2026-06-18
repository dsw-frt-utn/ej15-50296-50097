namespace Dsw2026Ej15.Api.Midleware;

using Dsw2026Ej15.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false; // No es nuestro error, que siga buscando otro handler
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/json";

        var errorResponse = new { error = validationException.Message };
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true; // Excepción manejada exitosamente
    }
}