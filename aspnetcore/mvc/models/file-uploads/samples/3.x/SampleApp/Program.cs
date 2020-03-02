using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace SampleApp
{
    public class Program
    {
        private static string certPassword { get; set; }

        public static void Main(string[] args)
        {
            using (var sr = new StreamReader("/kvmnt/CertPassword"))
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
