using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System;

namespace SampleApp
{
    public class Program
    {
        private static string certPassword { get; set; }
        private static string certLocation { get; set; }

        public static void Main(string[] args)
        {
            certPassword = "xyzzy";

            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appSettings.json", optional: true)
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();
                var certPasswordLocation = config.GetValue<string>("CertPasswordLocation");
                certLocation = config.GetValue<string>("CertLocation");

                using (var sr = new StreamReader(certPasswordLocation))
                {
                    certPassword = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting configuration: {ex.Message}");
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.ListenAnyIP(443, listenOptions =>
                            {
                                listenOptions.UseHttps("/state/certvol/golive.pfx", certPassword);
                            });
                        });
                });
    }

}
