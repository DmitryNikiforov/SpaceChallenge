using System;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Configuration.Client;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpaceHerders.Services;

namespace SpaceHerders.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddSwaggerGen();
            services.AddSingleton<IUsersLocationService, InMemoryUsersLocationService>();
            services.AddTransient<ICrowdsourcedPointsService, CouchbaseCrowdsourcedPointsService>();

            var config = new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://104.236.220.12:8091/pools") },
                BucketConfigs = new Dictionary<string, BucketConfiguration>
                {
                    ["geodb"] = new BucketConfiguration
                    {
                        BucketName = "geodb", Password = "123456",
                    }
                }

            };
            var cluster = new Cluster(config);
            var bucket = cluster.OpenBucket();
            services.AddInstance(bucket);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwaggerGen();
            app.UseSwaggerUi();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
