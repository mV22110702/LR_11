namespace LR_11.Loggers.FileLogger
{
    public static class AddFileLoggerProvider
    {
        public static void AddFile(this ILoggingBuilder loggingBuilder, string path)
        {
            loggingBuilder.AddProvider(new FileLoggerProvider(path));
        }
    }
}
