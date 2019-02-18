using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reservations.Api.Middlewares;
using Reservations.Application.Interfaces;
using Reservations.Application.Reservations;
using Reservations.Data.Db;
using Serilog;

namespace Reservations.WebApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            Log.Information("Start Service Registration");

            services.AddSingleton<IDatabase, InMemoryDatabase>();
            services.AddTransient<IReservationsService, ReservationsService>();

            services.AddSingleton(DatabaseFactory.CreateDatabase());

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:50544")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            Log.Information("End Service Registration");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Information("Start Service Configuration");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("default");

            app.UseMiddleware<CustomErrorMiddleware>();
            app.UseHttpsRedirection();
            app.UseMvc();
            Log.Information("End Service Configuration");
        }
    }
}
