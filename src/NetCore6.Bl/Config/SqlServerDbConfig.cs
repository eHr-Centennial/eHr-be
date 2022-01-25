using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore6.Model.Contexts;

namespace NetCore6.Bl.Config
{
    public static class SqlServerDbConfig
    {
        public static IServiceCollection ConfigSqlServerDbContext(this IServiceCollection services, string connection)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            return services;
        }
    }
}