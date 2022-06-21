using SurveyPerson.Api.Middleware;

namespace SurveyPerson.Api.Extensions;

public static class ExceptionHandlerMiddlewareExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)  
    {  
        app.UseMiddleware<ErrorsHandleMiddleware>();  
    }  
}