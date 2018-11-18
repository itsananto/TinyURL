using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TinyURL.Models;
using AutoMapper;
using TinyURL.Mapping;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using TinyURL.Services;

namespace TinyURL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();

            services.AddDbContext<TinyURLContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TinyURLContext")));


            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<URLMapsService>().As<IURLMapsService>();

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=URLMaps}/{action=Index}/{url?}");

                routes.MapRoute(
                       name: "encoded-url",
                       template: "{encoded}",
                       defaults: new { controller = "URLMaps", action = "Index" }
                    );
            });
        }
    }
}
