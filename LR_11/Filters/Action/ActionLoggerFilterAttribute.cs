using LR_11.Loggers.FileLogger;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace LR_11.Filters.Action
{
    public class ActionLoggerFilterAttribute : LogFilterAttributeBase<CustomerCountLoggerFilterAttribute>, IActionFilter
    {

        public ActionLoggerFilterAttribute(IConfiguration configuration): base(configuration.GetSection("ACTION_LOG_FILE_PATH").Value ?? "./action_log.txt")
        {
        }

        private void LogMethod(string? actionMethod)
        {
            if (actionMethod == null)
            {
                Logger.LogError("Action method to log is null");
                return;
            }

            Logger.LogInformation(actionMethod);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            LogMethod(context.ActionDescriptor.DisplayName);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
