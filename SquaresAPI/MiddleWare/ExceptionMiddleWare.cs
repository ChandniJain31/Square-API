using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SquaresAPI.BusinessLogic.Helpers;
using System.Net;

namespace SquaresAPI.MiddleWare
{
 
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            this._logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleGlobalExceptionAsync(context, ex);
            }
        }

        private async Task HandleGlobalExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            Type type = ex.InnerException != null ? ex.InnerException.GetType() : ex.GetType();
            switch(type.Name)
            {
                case nameof(SecurityTokenException): 
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case nameof(AppException):
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case nameof(KeyNotFoundException):
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            string result = JsonConvert.SerializeObject(new { error = ex.Message });
            _logger.LogError(ex,
               "An error occurred in Controller:" + context.Request.RouteValues["controller"]
               + ", ActionMethod:" + context.Request.RouteValues["action"] +
               ", ErrorMessage:" + ex.Message);
            await response.WriteAsync(result);
        }
    }
}
