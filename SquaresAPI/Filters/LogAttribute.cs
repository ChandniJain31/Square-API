using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace SquaresAPI.Filters
{
    public class LogAttribute :ActionFilterAttribute
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();      
        
        public bool AllowMultiple
        {
            get { return true; }
        }   
        
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _logger.Info($"Controller {actionContext.ActionDescriptor.RouteValues["controller"]}, Action Method {actionContext.ActionDescriptor.RouteValues["action"]} executing.");
        }
        
        public override void OnActionExecuted(ActionExecutedContext actionContext)
        {
            _logger.Info($"Controller {actionContext.ActionDescriptor.RouteValues["controller"]}, Action Method {actionContext.ActionDescriptor.RouteValues["action"]} executed.");
        }
    }
}
