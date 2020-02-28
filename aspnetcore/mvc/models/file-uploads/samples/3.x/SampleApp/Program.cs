using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
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
            Console.WriteLine($"The cert password is: {certPassword}");

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
                                Console.WriteLine($"The cert password is: {certPassword}");
                                listenOptions.UseHttps("/certvol/golive.pfx", "TJ&F[3=0wp=,1<ECHKna");
                            });
                        });
                });
    }
}
