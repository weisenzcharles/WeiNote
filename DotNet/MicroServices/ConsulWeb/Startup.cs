using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsulWeb.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsulWeb
{
    public class Startup
    {
        //public IConfiguration Configuration { get; }

        //// This method gets called by the runtime. Use this method to add services to the container.
        //// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddOptions().AddConsul(option =>
        //    {
        //        option.WithHost(Configuration["ConsulConfiguration:Host"]);
        //    }).AddMvc();
        //}

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime, IServiceProvider svp)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseConsul(lifetime, option =>
            {
                option.WithSelfHost(Configuration["SelfHost"]);
                option.WithServerName(Configuration["ConsulConfiguration:ServerName"]);
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions().AddConsul(option =>
            {
                option.WithHost(Configuration["ConsulConfiguration:Host"]);
            }).AddMvc();
        }

        //public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime, IServiceProvider svp)
        //{
        //    if (env.IsDevelopment())
        //        app.UseDeveloperExceptionPage();
        //    else
        //        app.UseExceptionHandler("/Home/Error");

        //    app.UseConsul(lifetime, option =>
        //    {
        //        option.WithSelfHost(Configuration["SelfHost"]);
        //        option.WithServerName(Configuration["ConsulConfiguration:ServerName"]);
        //    });
        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapGet("/", async context =>
        //        {
        //            await context.Response.WriteAsync("Hello World!");
        //        });
        //    });
        //    //app.UseMvc(routes =>
        //    //{
        //    //    routes.MapRoute(
        //    //        "default",
        //    //        "{controller=Home}/{action=Index}/{id?}");
        //    //});

        //}

    }
}
