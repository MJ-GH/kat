using System;
using kat.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(kat.Areas.Identity.IdentityHostingStartup))]
namespace kat.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<katLoginExampleContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("katDBConnectionString")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<katLoginExampleContext>();

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequireAuthenticatedUser", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                    });
                    options.AddPolicy("RequireAdminUser", policy => 
                    {
                        policy.RequireRole("Admin");
                    });
                });
            });
        }
    }
}