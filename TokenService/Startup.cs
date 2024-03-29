﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using TokenService.Core.Repository;
using TokenService.Core.Service;
using TokenService.InMemory.Repository;
using TokenService.Model.Entity;

namespace TokenService
{
    /// <summary>
    /// The cannonical startup class.
    /// <para></para>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments#startup-conventions
    /// Startup classes can determine their runtime environment or be named with the environment as a suffix
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Standard boilerplate that loads the application settings / configuration
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// Configuration Properties contianer
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

            // appsettings.json: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x
            // appsettings.json was defaults Level "Warning" by template.  Changed to Level to "Information"
            // Sends to Debug window at Information or higher. Overriding because we want Debug window at Debug if available 
            // Debug output only visible when running with IIS Express because 
            // ASPNETCORE_ENVIRONMENT:Development mode only set for iis profile in launchsettings.json
            // launchsettings.json https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments


            // https://stackoverflow.com/questions/53840298/how-to-fix-obsolete-iloggerfactory-methods
            services.AddLogging(loggingBuilder =>
           {
               loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
               loggingBuilder.AddConsole();
               loggingBuilder.AddDebug();
           });

            // DI container - AddTransient(), AddScoped(), AddSingleton()
            // persistance
            // services
            services.AddTransient<ITokenOperationsService, TokenOperationsService>();
            // a single repository for all requests
            services.AddSingleton<IRepository<TokenEntity>, TokenInMemRepository>();
            // controllers

            // Add framework services.
            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            // from https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Token Service API", Version = "v1" });

                // Configure swagger to pull REST controller method comments.
                // Set the comments path for the Swagger JSON and UI. Relies on the generation of XML documentation
                // XML documentation must be enabled with this file name in the TokenService properties 
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "TokenService.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <para></para>
        /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup 
        /// IHostingEnvironment and ILoggerFactory may be specified in method signature
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param> https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-net-core-3-0-preview-4/
            app.UseStaticFiles();
            // Runs matching. An endpoint is selected and set on the HttpContext if a match is found.
            app.UseRouting();

            // Middleware that run after routing occurs. Usually the following appear here:
            //app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseCors();

            // Executes the endpoint that was selected by routing.
            app.UseEndpoints(endpoints =>
            {
                // Mapping of endpoints goes here:
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
