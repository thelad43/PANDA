[assembly: Microsoft.AspNetCore.Hosting.HostingStartup(typeof(Panda.Web.Areas.Identity.IdentityHostingStartup))]

namespace Panda.Web.Areas.Identity
{
    using Microsoft.AspNetCore.Hosting;

    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}