using NLog.Web;

namespace SquaresAPI
{
    public static class NLogExtensions
    {
        public static void ConfigureNLog(this WebApplicationBuilder builder)
        {
            var logpath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            NLog.GlobalDiagnosticsContext.Set("LogDirectory", logpath);
            var nlogconfigpath = string.Concat(Directory.GetCurrentDirectory(), "/NLog.config");
            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.AddNLog(nlogconfigpath);
                logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            }).UseNLog();
        }
    }
}
