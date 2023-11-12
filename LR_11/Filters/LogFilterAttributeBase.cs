using LR_11.Filters.Action;
using LR_11.Loggers.FileLogger;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LR_11.Filters
{
    public abstract class LogFilterAttributeBase<T> : Attribute where T : IFilterMetadata
    {
        
        protected LogFilterAttributeBase(string path)
        {
            LogPath = path;
            Logger = initLogger(path);
        }

        protected ILogger<T> Logger { get; set; }
        protected string LogPath { get; }
        protected ILogger<T> initLogger(string path) 
        {

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFile(path);
            });
            var logger = loggerFactory.CreateLogger<T>();
            return logger;
        }

        
    }
}
