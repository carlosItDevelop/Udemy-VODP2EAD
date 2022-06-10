using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace VOD.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuider(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuider(string[] args) =>
            Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuider =>
                    {
                        webBuider.UseStartup<Startup>();
                    });
    }
}
