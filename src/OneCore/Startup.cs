using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneCore.Data;
using OneCore.Data.Models;
using OneCore.Extensions;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace OneCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Setup Database
            string connectionString = Configuration.GetConnectionString("DatabaseConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("OneCore")));

            services.AddAutoMapper(typeof(Startup));
            services.AddHealthChecks();

            // Setup Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigurePasswordRule();
            services.AddJwtAuthentication(Configuration);

            services.AddControllers()
                .AddNewtonsoftJson(j =>
                {
                    j.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };

                    j.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestResponseLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Always use HTTPS redirection
            app.UseHttpsRedirection();

            // Handle Chrome DevTools specific requests
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/.well-known/appspecific"))
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{}");
                    return;
                }
                await next();
            });

            // Serve static files from wwwroot/browser
            app.UseDefaultFiles();

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OneCore API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Handle SPA routing
                endpoints.MapFallbackToFile("index.html");
            });

            app.UseHealthChecks("/healthcheck");
        }
    }
}
