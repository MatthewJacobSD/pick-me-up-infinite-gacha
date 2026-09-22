using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PickMeUp.Api.Common.Errors;

public static class ProblemDetailsExtensions
{
    private const string BaseUri = "https://pickmeup/errors/";

    public static IApplicationBuilder UseDomainExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception is null)
                    return;

                var (statusCode, type, detail) = exception switch
                {
                    VersionConflictException ex => (409, $"{BaseUri}version-conflict", ex.Message),
                    NotFoundException ex          => (404, $"{BaseUri}not-found", ex.Message),
                    UnauthorizedAccessException   => (403, $"{BaseUri}policy-denied", "Access to the requested resource is denied."),
                    ValidationException ex        => (400, $"{BaseUri}validation", ex.Message),
                    FluentValidation.ValidationException ex => (400, $"{BaseUri}validation", FormatFluentValidationErrors(ex)),
                    DomainException ex            => (400, $"{BaseUri}validation", ex.Message),
                    _                            => (500, $"{BaseUri}unexpected", "An unexpected error occurred."),
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = GetTitle(statusCode),
                    Detail = detail,
                    Type = type,
                };

                if (exception is ValidationException validationEx)
                {
                    problemDetails.Extensions["errors"] = validationEx.Errors;
                }

                if (exception is FluentValidation.ValidationException fluentEx)
                {
                    problemDetails.Extensions["errors"] = fluentEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray());
                }

                await context.Response.WriteAsJsonAsync(problemDetails);
            });
        });

        return app;
    }

    private static string FormatFluentValidationErrors(FluentValidation.ValidationException ex)
    {
        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .Select(g => $"{g.Key}: {string.Join("; ", g.Select(e => e.ErrorMessage))}");

        return $"Validation failed. {string.Join(" | ", errors)}";
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        _   => "Error",
    };
}
