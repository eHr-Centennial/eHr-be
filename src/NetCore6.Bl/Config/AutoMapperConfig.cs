using Microsoft.Extensions.DependencyInjection;

namespace NetCore6.Bl.Config
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection ConfigAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}