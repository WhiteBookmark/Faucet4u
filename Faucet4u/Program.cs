using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Entities;
using MongoDB.Driver.Linq;
using System;
using System.Linq;

namespace Faucet4u
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).UseUrls("https://localhost:5000").Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}