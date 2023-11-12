using System.Text.Json;

namespace LR_11.Loggers.FileLogger
{
    public class FileLogger : ILogger
    {
        private string _categoryName;
        private string _path;

        public FileLogger(string path, string categoryName)
        {
            _path = path;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;
        
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> format)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

          
            File.AppendAllText(_path, $"[{_categoryName}, {logLevel.ToString()}{(exception != null ? ", Exception":"") }] {DateTime.UtcNow} (UTC): {format(state,exception)}{Environment.NewLine}");
        }
    }
}
