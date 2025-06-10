using Microsoft.AspNetCore.Mvc;
using Speedex.Domain.Commons;

namespace Speedex.Api.Features.Commons;

public static class ResponseMapper
{
    
    public static ProblemDetails ToBadRequest(this IReadOnlyList<ValidationError> errors)
    {
        var summary = $"{errors.Count} validation error(s) occurred.";

        return new ProblemDetails
        {
            Type = "about:blank",
            Status = 400,
            Title = "Bad Request",
            Detail = summary,
            Extensions =
            {
                {
                    "reasons", errors.Select(e => new
                    {
                        code = e.Code,
                        message = e.Message
                    }).ToArray()
                }
            }
        };
    }

    public static ProblemDetails ToInternalServerError(this TechnicalError error)
    {
        return new ProblemDetails
        {
            Type = "about:blank",
            Status = 500,
            Title = "Internal Server Error",
            Detail = "A technical error occurred while processing your request.",
            Extensions =
            {
                {
                    "reasons", new[]
                    {
                        new
                        {
                            code = error.Code,
                            message = error.Message
                        }
                    }
                }
            }
        };
    }
}