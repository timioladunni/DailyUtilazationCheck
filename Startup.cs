using DailyUtilizationCheck.AeonData;
using DailyUtilizationCheck.Infrastructure;
using DailyUtilizationCheck.Interface;
using DailyUtilizationCheck.Model;
using DailyUtilizationCheck.Service;
using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DailyUtilizationCheck
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
           
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangFireString")));
            GlobalConfiguration.Configuration
           .UseSqlServerStorage(Configuration.GetConnectionString("HangFireString")).WithJobExpirationTimeout(TimeSpan.FromDays(7));
            services.AddHangfireServer();
           
            services.AddCors(options => options.AddPolicy("AllowEverything", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
           
            services.AddDbContext<Aeon2Context>(opt => opt.UseSqlServer
          (Configuration.GetConnectionString("AeonString")));
            services.AddDbContext<HangfireContext>(opt => opt.UseSqlServer
         (Configuration.GetConnectionString("HangFireString")));
            services.Configure<server>(Configuration.GetSection("server"));
            services.AddScoped<IUtilization, UtilizationService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DailyUtilization API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddControllers(options => options.Filters.Add(typeof(Class)))
                 .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "CREDEQUITY API V1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();
           
            app.UseAuthorization();

            //Basic Authentication added to access the Hangfire Dashboard  
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                AppPath = null,
                DashboardTitle = "Hangfire Dashboard",
                Authorization = new[]
                {
                    new HangfireCustomBasicAuthenticationFilter{
                    User = Configuration.GetSection("HangfireCredentials:UserName").Value,
                    Pass = Configuration.GetSection("HangfireCredentials:Password").Value
                }
                }
                //Authorization = new[] { new DashboardNoAuthorizationFilter() },  
                //IgnoreAntiforgeryToken = true  
            });

           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
