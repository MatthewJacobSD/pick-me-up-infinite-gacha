using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using PickMeUp.Api.Common.Errors;

namespace PickMeUp.Api.Social;

public sealed class SocialExceptionHandler : IExceptionHandler
{
    public const string TypeBase = "https://pickmeup/errors/";

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var mapped = Map(exception);
        if (mapped is null)
            return false;

        var (status, type, title, code) = mapped.Value;
        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = "application/problem+json";
        await JsonSerializer.SerializeAsync(httpContext.Response.Body, new
        {
            type,
            title,
            status,
            detail = exception.Message,
            code
        }, cancellationToken: cancellationToken);
        return true;
    }

    private static (int Status, string Type, string Title, string Code)? Map(Exception exception)
    {
        return exception switch
        {
            SocialValidationException => (400, TypeBase + "validation", "Bad Request", "social.validation"),
            ValidationException => (400, TypeBase + "validation", "Bad Request", "social.validation"),
            SocialUnauthenticatedException => (401, TypeBase + "unauthorized", "Unauthorized", "social.unauthenticated"),
            SocialPolicyDeniedException denied => (403, TypeBase + "policy-denied", "Forbidden", denied.Code),
            UnauthorizedAccessException => (403, TypeBase + "policy-denied", "Forbidden", "social.policy_denied"),
            SocialNotFoundException => (404, TypeBase + "not-found", "Not Found", "social.not_found"),
            NotFoundException => (404, TypeBase + "not-found", "Not Found", "social.not_found"),
            SocialConflictException conflict => (409, TypeBase + "version-conflict", "Conflict", conflict.Code),
            VersionConflictException => (409, TypeBase + "version-conflict", "Conflict", "social.conflict"),
            _ => null
        };
    }
}
