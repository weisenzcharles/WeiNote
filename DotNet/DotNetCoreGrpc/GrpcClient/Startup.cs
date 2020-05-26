using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static GrpcService.Greeter;

namespace GrpcClient
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpcClient<GreeterClient>(options => options.Address = new Uri("https://localhost:5001"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    GreeterClient client = context.RequestServices.GetService<GreeterClient>();
                    HelloRequest request = new HelloRequest();
                    request.Name = "Charles";
                    var reply = await client.SayHelloAsync(request);

                    await context.Response.WriteAsync(reply.Message);
                });
            });
        }
    }
}
