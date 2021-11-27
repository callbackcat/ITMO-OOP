using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace BackupsExtra.Tools.Loggers
{
    public static class LoggerHolder
    {
        private static ILogger _log;

        public static ILogger Instance => _log ??= Create();

        private static ILogger Create()
        {
            Logger log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.File("log.txt")
                .CreateLogger();

            log.Information("New logger was initiated");

            return log;
        }
    }
}