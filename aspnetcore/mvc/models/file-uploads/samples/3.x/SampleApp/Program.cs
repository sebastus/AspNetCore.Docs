using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace SampleApp
{
    public class Program
    {
        private static string certPassword { get; set; }
        private static string certLocation { get; set; }

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false)
                .Build();
            var certPasswordLocation = config.GetValue<string>("CertPasswordLocation");
            certLocation = config.GetValue<string>("CertLocation");

            using (var sr = new StreamReader(certPasswordLocation))
            {
                certPassword = sr.ReadToEnd();
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
                            serverOptions.ListenAnyIP(5001, listenOptions =>
                            {
                                listenOptions.UseHttps("/certvol/golive.pfx", certPassword);
                            });
                        });
                });
    }

}
