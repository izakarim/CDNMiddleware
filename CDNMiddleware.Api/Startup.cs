using System;
using System.Text.RegularExpressions;
using CDNMiddleware.Application.Extensions;
using CDNMiddleware.DataAccess.Extensions;
using CDNMiddleware.DataAccess.Helpers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using CDNMiddleware.DataAccess.Models;
using CDNMiddleware.DataAccess;

namespace CDNMiddleware.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            //https://stackoverflow.com/a/46364665/3335442
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(cors =>
            {
                cors.AddPolicy("AllowOrigin", options =>
                {
                    options
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            ;
                });
            });

            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                        /* For SnakeCase Enabling - please ensure all API allowed reading for snake case
                        //https://stackoverflow.com/questions/42353847/how-to-properly-set-up-snake-case-json-for-dotnet-core-api
                            options.SerializerSettings.ContractResolver = new DefaultContractResolver
                            {
                             NamingStrategy = new SnakeCaseNamingStrategy()
                            }; 
                        */

                    }
                    );

            //https://www.c-sharpcorner.com/blogs/customizing-model-validation-response-resulting-as-http-400-in-net-core
            services.AddMvc()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        return actionContext.CustomizeErrorResponse().asResult();
                    };
                });

            services.AddOptions();
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 90000000; // 90MB
            });

            services.Configure<SettingModel>(options => Configuration.GetSection("ConnectionStrings").Bind(options));
            services.AddHttpContextAccessor();

            #region Database Connection Configuration
            services.AddTransient<CDNMiddlewareDbContext>();
            services.AddDbContextFactory<CDNMiddlewareDbContext>(
                options =>
                    options.UseMySql(
                        Configuration.GetConnectionString("DB_CONN"),
                        new MariaDbServerVersion(new Version(1, 2, 3))
                    )
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .EnableDetailedErrors()
            );
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CDNMiddleware.Api", Version = "v1" });
            });

            services.AddHttpContextAccessor();

            services.RegisterDataAccess()
                    .RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CDNMiddleware.Api v1"));
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePages(async context =>
            {
                HttpContext _httpContext = context.HttpContext;

                if (_httpContext.Request.GetTypedHeaders().AcceptHeaderIsJson())
                {
                    _httpContext.Response.ContentType = "application/json";

                    await context.HttpContext.Response.WriteAsJsonAsync(ApiResponse.byStatusCode(_httpContext.Response.StatusCode));
                }
            });

            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowOrigin");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller}/{action}/{id?}"
                        );

                endpoints.MapRazorPages();
            });
        }
    }
}

