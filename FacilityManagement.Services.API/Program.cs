using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FacilityManagement.Services.API
{
    /// <summary>
    /// Program class
    /// </summary>
    public class Program {
        /// <summary>
        /// Program main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) 
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Application starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "The application failed to start correctly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        
        /// <summary>
        /// Create Host Builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()//logs errors using serilog error logging package
                .ConfigureWebHostDefaults((webBuilder) => 
                {
                    webBuilder.UseUrls($"http://+:{HerokuDatabaseSetup.HostPort}");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
