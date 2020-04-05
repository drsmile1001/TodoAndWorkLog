using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace TodoAndWorkLog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.local.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.local.json", optional: true)
                .Build();
            var logConfig = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Information ||
                    Matching.FromSource("TodoAndWorkLog")(e))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Warning ||
                        Matching.FromSource("EventServer")(e))
                    .WriteTo.File(new CompactJsonFormatter(), @"Logs\.clef", rollingInterval: RollingInterval.Day, shared: true));

            Log.Logger = logConfig.CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;
                    builder.AddJsonFile("appsettings.json", optional: false)
                           .AddJsonFile("appsettings.local.json", optional: true)
                           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                           .AddJsonFile($"appsettings.{env.EnvironmentName}.local.json", optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
