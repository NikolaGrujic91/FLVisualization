using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FLVisualization.DAL.EF;
using FLVisualization.DAL.Repos;
using FLVisualization.DAL.Repos.Interfaces;


namespace FLVisualization.Service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvcCore()
                .AddJsonFormatters(j =>
                {
                    j.ContractResolver = new DefaultContractResolver(); // To Pascal Casing
                    j.Formatting = Formatting.Indented; // Indented formatting
                });

            // Allow all http request types
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
                });
            });

            // Configure EF Core
            services.AddDbContext<FLVisualizationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FLVisualization")));

            // Configure the Dependency Injection Container
            services.AddScoped<ITeamRepo, TeamRepo>();
            services.AddScoped<IPositionRepo, PositionRepo>();
            services.AddScoped<IPlayerRepo, PlayerRepo>();
            services.AddScoped<IPlayerHistoryRepo, PlayerHistoryRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            app.UseMvc();
        }
    }
}
