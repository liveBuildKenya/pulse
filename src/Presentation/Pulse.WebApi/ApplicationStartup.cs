using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pulse.WebApi.Assertion;
using Pulse.WebApi.Attestation;
using Pulse.WebAuthn.Application.Infrastructure;
using Pulse.WebAuthn.Domain.Infrastructure.Migrations;
using Scalar.AspNetCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pulse.WebApi
{
    /// <summary>
    /// Represents the application startup class
    /// </summary>
    public class ApplicationStartup
    {
        #region Constructor

        public ApplicationStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the application services to the service container
        /// </summary>
        /// <param name="serviceCollection">Service container</param>
        [Obsolete]
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddOpenApi();
            serviceCollection.AddMagicAuthApplicationServices(Configuration);


            var origins = new[] { "https://localhost:7076" };
            var originsHashSet = new HashSet<string>(origins);

            serviceCollection.AddFido2(options =>
            {
                options.ServerDomain = "localhost";
                options.ServerName = "Fido Server";
                options.Origins = originsHashSet;
                options.TimestampDriftTolerance = 300000;
                options.MDSCacheDirPath = "";
            });

            serviceCollection.AddDistributedMemoryCache();

            // Allow the Blazor WebAssembly client origin and allow credentials (cookies)
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy("AllowClient", builder =>
                    builder.WithOrigins(origins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .WithExposedHeaders("X-Assertion-Options-Key"));
            });
        }

        /// <summary>
        /// Configures the application HTTP request pipeline
        /// </summary>
        /// <param name="applicationBuilder">Application builder</param>
        /// <param name="webHostEnvironment">Web host environment</param>
        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                MigrationExtensions.RunMigrations(scope.ServiceProvider);
            }
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseRouting();
            // Apply CORS policy before endpoints so cross-origin requests from the SPA are allowed
            applicationBuilder.UseCors("AllowClient");
            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapAttestationRoutes();
                endpoints.MapAssertionRoutes();
                endpoints.MapOpenApi();
                endpoints.MapScalarApiReference();

                // Redirect root "/" to Scalar
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/scalar", permanent: false);
                    return Task.CompletedTask;
                });
            });
        }

        #endregion
    }
}
