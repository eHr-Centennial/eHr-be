using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Net.Http.Headers;
using NetCore6.Api.Config;
using NetCore6.Bl.Config;
using NetCore6.Bl.IoC;
using NetCore6.Core.IoC;
using NetCore6.Core.Settings;
using NetCore6.Model.IoC;
using NetCore6.Services.IoC;

namespace NetCore6.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            this.Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            #region Adding Settings Sections
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            #endregion
            
            #region  CORS
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            
            services.AddCors(options => 
            {
                options.AddPolicy("AllowAllPolicy", 
                    builder => 
                    {
                        builder
                            .WithOrigins(appSettings.ClientUrls)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();

                        //TODO: remove this line for production
                        builder.SetIsOriginAllowed(x => true);
                    });
            });
            #endregion
            
            #region External Dependencies
            services.ConfigSqlServerDbContext(Configuration.GetConnectionString("DefaultConnection"));

            services.AddControllers(options => options.EnableEndpointRouting = false)
                .ConfigFluentValidation()
                .AddOData(options => 
                {
                    options.Select().Expand().Filter().OrderBy().SetMaxTop(100).Count();
                })
                .AddNewtonsoftJson();
                
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
            
            services.ConfigAutoMapper();
            #endregion
                
            services.AddEndpointsApiExplorer();

            #region API Libraries
            services.ConfigJwtAuth(Configuration);
            services.ConfigSwagger();
            services.ConfigIdentity();
            #endregion

            #region IoC Registry
            services.AddServiceRegistry();
            services.AddBlRegistry();
            services.AddModelRegistry();
            services.AddCoreRegistry();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endPoints => 
            {
                endPoints.MapControllers();
            });

        }
    }
}