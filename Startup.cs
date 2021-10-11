using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eis.Identity.Api.AsyncDataServices;
using Eis.Identity.Api.Data;
using Eis.Identity.Api.SyncDataServices.Grcp;
using Eis.Identity.Api.SyncDataServices.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Eis.Identity.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("--> Using SQL Server context.");
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConn")));

            services.AddScoped<IAppUserRepo, AppUserRepo>();
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddGrpc();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eis.Identity.Api", Version = "v1" });
            });

            Console.WriteLine($"--> IdentityService Endpoint {Configuration["IdentityService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eis.Identity.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcIdentityService>();
                endpoints.MapGet("/Protos/identity.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/identity.proto"));
                });
            });

            PrepDb.PrepPopulation(app);
        }
    }
}
