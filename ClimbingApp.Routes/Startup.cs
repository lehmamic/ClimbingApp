using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ClimbingApp.Routes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton((serviceProvider) =>
            {
                return new DocumentStore { Urls = new[] { "http://localhost:8080" } }.Initialize();
            });

            services.AddScoped((serviceProvider) => {
                var store = serviceProvider.GetService<IDocumentStore>();
                return store.OpenAsyncSession(new Raven.Client.Documents.Session.SessionOptions
                {
                    Database = "ClimbingRoutes"
                });
            });

            services.AddSingleton((serviceProvider) =>
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.AddMaps(typeof(Startup).Assembly);
                });

                config.AssertConfigurationIsValid();

                return config;
            });

            services.AddSingleton<IMapper>((serviceProvider) =>
            {
                var config = serviceProvider.GetService<MapperConfiguration>();
                return new Mapper(config);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
