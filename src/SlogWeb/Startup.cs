using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SlogWeb.Data;
using Microsoft.EntityFrameworkCore;
using SlogWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SlogWeb.Services;
using AutoMapper;

namespace SlogWeb {
    public class Startup {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            if (env.IsDevelopment()) {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddOptions();
            services.Configure<AdminOptions>(Configuration.GetSection("Admin"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper();
            services.AddSingleton<DbSeeder>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DbSeeder dbSeeder) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            if (env.IsProduction()) {
                app.UseForwardedHeaders(new ForwardedHeadersOptions {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "new_comment",
                    template: "comments/{postId}",
                    defaults: new { controller = "Comments", action = "Create" }
                );
                routes.MapRoute(
                    name: "sessions",
                    template: "account/{action}",
                    defaults: new { controller = "Account", action = "Login" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller}",
                    defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(
                    name: "new_route",
                    template: "{controller}/new",
                    defaults: new { action = "Create" }
                );
                routes.MapRoute(
                    name: "details",
                    template: "{controller}/{id}",
                    defaults: new { action = "Details" });
                routes.MapRoute(
                    name: "other_actions",
                    template: "{controller}/{id}/{action}");
            });

            try {
                dbSeeder.SeedAsync().Wait();
            } catch (AggregateException ex) {
                throw new Exception(ex.ToString());
            }
        }
    }
}
