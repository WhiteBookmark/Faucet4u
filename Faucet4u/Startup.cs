using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Faucet4u
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
            services.AddDistributedMemoryCache();

            services.AddHttpContextAccessor();

            services.AddCors();
            services.AddMvc()
                           .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                           .AddMvcOptions(options =>
                           {
                               options.Filters.Add(typeof(ValidatorActionFilter));
                               options.MaxModelValidationErrors = 50;
                           });

            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build());
        }

        // This method gets called by the runtime. Uses this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                //KnownNetworks = { new IPNetwork(IPAddress.Parse("172.64.0.0"), 13) }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                    builder.WithOrigins(new string[]
                    { "http://localhost:8080",
                        "http://localhost:8081",
                        "https://localhost:5001",
                        "http://ptp.faucet4all.com",
                        "http://faucet4all.com",
                        "http://admin.faucet4all.com" })
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                   );
            app.UseMvc();
        }
    }
}
