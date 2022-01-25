using Microsoft.AspNetCore.Identity;
using NetCore6.Model.Contexts;

namespace NetCore6.Api.Config
{
    public static class IdentityConfig
    {
        public static IServiceCollection ConfigIdentity(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}