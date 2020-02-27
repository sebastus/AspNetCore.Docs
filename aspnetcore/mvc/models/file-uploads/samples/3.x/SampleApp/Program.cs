using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("https://localhost:5001/;http://localhost:5000/")
                        .ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.Listen(IPAddress.Parse("0.0.0.0"), 5000);
                            serverOptions.Listen(IPAddress.Parse("0.0.0.0"), 5001,
                                listenOptions =>
                                {
                                    listenOptions.UseHttps("golive.pfx", "TJ&F[3=0wp=,1<ECHKna");
                                });
                        })
                        .UseStartup<Startup>();
                });
    }
}
