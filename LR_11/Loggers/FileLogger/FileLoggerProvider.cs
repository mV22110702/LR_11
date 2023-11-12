namespace LR_11.Loggers.FileLogger
{
    [ProviderAlias("FileProvder")]
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _path;
        public FileLoggerProvider(string path)
        {
            _path = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path,categoryName);
        }

        public void Dispose()
        {
        }
    }
}
