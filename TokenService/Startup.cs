using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using TokenService.Model.Entity;
using TokenService.Repository;
using TokenService.Service;

namespace TokenService
{
    /// <summary>
    /// The cannonical startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Standard boilerplate that loads the application settings / configuration
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// boilerplate
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CryptographySettings>(Configuration.GetSection("CryptographySettings"));
            services.AddLogging();

            // DI container - AddTransient(), AddScoped(), AddSingleton()
            // persistance
            // services
            services.AddTransient<ITokenCreationService, TokenCreationService>();
            services.AddTransient<ITokenValidationService, TokenValidationService>();

            services.AddSingleton<IRepository<TokenEntity>, TokenInMemRepository>();
            // controllers

            // Add framework services.
            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            // from https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Token Service API", Version = "v1" });

                // Configure swagger to pull REST controller method comments.
                // Set the comments path for the Swagger JSON and UI. Relies on the generation of XML documentation
                // XML documentation must be enabled with this file name in the TokenService properties 
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "TokenService.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // appsettings.json: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x
            // appsettings.json was defaults Level "Warning" by template.  Changed to Level to "Information"
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            // Sends to Debug window at Information or higher. Overriding because we want Debug window at Debug if available 
            // Debug output only visible when running with IIS Express because 
            // ASPNETCORE_ENVIRONMENT:Development mode only set for iis profile in launchsettings.json
            // launchsettings.json https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments
            loggerFactory.AddDebug(LogLevel.Debug);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}
