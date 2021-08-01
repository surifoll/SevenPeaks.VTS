using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace SevenPeaks.VTS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();  
            //Initialize Logger    
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();  
            try {  
                Log.Information("Application Starting.##############3");  
                CreateHostBuilder(args).Build().Run();  
            } catch (Exception ex) {  
                Log.Fatal(ex, "The Application failed to start.######333");  
            } finally {  
                Log.CloseAndFlush();  
            }  
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).UseSerilog() //Uses Serilog instead of default .NET Logger    
            .ConfigureWebHostDefaults(webBuilder => {  
                webBuilder.UseStartup < Startup > ();  
            }); 
    }
}
