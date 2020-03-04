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
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appSettings.json", optional: false)
                    .Build();

                var certPasswordLocation = config.GetValue<string>("CertPasswordLocation");
                using (var sr = new StreamReader(certPasswordLocation))
                {
                    certPassword = sr.ReadToEnd();
                }

                certLocation = config.GetValue<string>("CertLocation");
                Console.WriteLine($"cert location is: {certLocation}");

                var filesLocation = config.GetValue<string>("StoredFilesPath");
                Console.WriteLine($"files location is: {filesLocation}");

                if (!Directory.Exists(filesLocation))
                {
                    Directory.CreateDirectory(filesLocation);
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
                                listenOptions.UseHttps(certLocation, certPassword);
                            });
                        });
                });
    }

}
