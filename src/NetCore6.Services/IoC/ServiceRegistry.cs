using Microsoft.Extensions.DependencyInjection;
using NetCore6.Services.Services;
using NetCore6.Services.Services.User;

namespace NetCore6.Services.IoC
{
    public static class ServiceRegistry
    {
        public static void AddServiceRegistry(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IExamplePersonService, ExamplePersonService>();
        }
    }
}