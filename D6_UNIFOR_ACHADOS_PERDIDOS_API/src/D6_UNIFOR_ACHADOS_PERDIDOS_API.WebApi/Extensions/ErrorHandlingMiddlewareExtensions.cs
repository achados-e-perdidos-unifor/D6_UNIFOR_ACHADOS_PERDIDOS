using D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.Middlewares;

namespace D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.Extensions
{
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
            => app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
