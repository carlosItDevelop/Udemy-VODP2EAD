using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(VOD.Admin.Areas.Identity.IdentityHostingStartup))]
namespace VOD.Admin.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}