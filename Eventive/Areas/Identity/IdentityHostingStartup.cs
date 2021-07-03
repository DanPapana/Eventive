using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Eventive.Areas.Identity.IdentityHostingStartup))]
namespace Eventive.Areas.Identity
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