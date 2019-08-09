using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Faucet4u.GlobalConnections.Helper.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Entities;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                           .AddJsonOptions(options =>
                           {
                               //This makes app return objects in PascalCase
                               options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                           })
                           .AddMvcOptions(options =>
                           {
                               options.Filters.Add(typeof(ValidatorActionFilter));
                               options.MaxModelValidationErrors = 50;
                           });
            services.AddMongoDBEntities("Faucet4all", "localhost", 27017);

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
            });

            if (env.IsDevelopment())
            {
                //DevelopmentStartup();
                ProductionStartup();
                app.UseDeveloperExceptionPage();
                app.UseCors(builder =>
                    builder.WithOrigins(new string[]
                    { "http://localhost:8080",
                        "http://localhost:8081",
                        "https://localhost:5001" })
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                   );
            }
            else if (env.IsProduction())
            {
                ProductionStartup();
                app.UseCors(builder =>
                    builder.WithOrigins(new string[]
                    {
                        "http://ptp.faucet4all.com",
                        "http://faucet4all.com",
                        "https://admin.faucet4all.com" })
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                   );
            }
            app.UseHsts();
            app.UseMvc();
        }

        private static void DevelopmentStartup()
        {
            try
            {
                //DB.Delete<Settings>(x => x.SettingsID.Equals(KeysVariable.SettingsKey));

                //Unique Username with Text type index while ascending/descending for SessionId
                DB.Index<Users>()
                    .Option(SetOption => SetOption.Unique = true)
                    .Option(SetOption => SetOption.Background = true)
                    .Key(User => User.Username, KeyType.Text)
                    .Key(User => User.SessionId, KeyType.Ascending)
                    .Create();

                //Logs index with Descending datetime
                DB.Index<Logs>()
                    .Option(SetOption => SetOption.Background = true)
                    .Key(Log => Log.DateTime, KeyType.Descending)
                    .Create();

                //Logs index with Descending datetime
                DB.Index<SupportTickets>()
                    .Option(SetOption => SetOption.Background = true)
                    .Key(Ticket => Ticket.DateTime, KeyType.Descending)
                    .Create();

                //Ensure our current settings exist in Settings collection
                //And insert if doesn't exist
                //No better method for "insert if not exist" so it is splitted into 2 queries in this case (Upsert won't work here)

                Guid SettingsExist = DB.Queryable<Settings>()
                    .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                    .Select(Setting => Setting.SettingsID)
                    .FirstOrDefault();
                if (!SettingsExist.Equals(KeysVariable.SettingsKey))
                {
                    Settings NewSetting = new Settings()
                    {
                        SettingsID = KeysVariable.SettingsKey
                    };
                    NewSetting.Save();
                }

                Guid RecordsExist = DB.Queryable<Records>()
                    .Where(Setting => Setting.RecordsID.Equals(KeysVariable.RecordsKey))
                    .Select(Setting => Setting.RecordsID)
                    .FirstOrDefault();
                if (!RecordsExist.Equals(KeysVariable.RecordsKey))
                {
                    Records NewSetting = new Records()
                    {
                        RecordsID = KeysVariable.RecordsKey
                    };
                    NewSetting.Save();
                }

                //Insert all banner networks if they don't already exist
                //Deliberately removed Parallel.ForEach because it was mixing up max id thus resulting in incorrect maxId
                foreach (KeyValuePair<string, string> NetworkList in HTMLCodeVariable.LocalhostNetworks)
                {
                    int MaxId = GetMaxId.Network(NetworkList.Key, 1);

                    DB.Update<Settings>()
                   .Match(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   .Match(Setting => !Setting.BannerNetwork.Any(Element => Element.HTMLCode.Equals(NetworkList.Value) && Element.Type.Equals(NetworkList.Key)))
                   .Modify(Filter => Filter.Push(Setting => Setting.BannerNetwork, new BannerNetworkModel
                   {
                       Id = MaxId,
                       HTMLCode = NetworkList.Value,
                       Type = NetworkList.Key
                   }))
                   .Execute();
                }

                //Add initial rotator data
                foreach (KeyValuePair<string, LocalhostRotator> Rotator in HTMLCodeVariable.LocalhostRotators)
                {
                    int MaxId = GetMaxId.Rotator(Rotator.Key, 1);

                    DB.Update<Settings>()
                   .Match(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   .Match(Setting => !Setting.BannerRotator.Any(
                       Element => Element.ImageLink.Equals(Rotator.Value.Image)
                   && Element.TargetLink.Equals(Rotator.Value.Target)
                   && Element.Type.Equals(Rotator.Key)))
                   .Modify(Filter => Filter.Push(Setting => Setting.BannerRotator, new BannerRotatorModel
                   {
                       Id = MaxId,
                       ImageLink = Rotator.Value.Image,
                       TargetLink = Rotator.Value.Target,
                       Credit = 100000
                   }))
                   .Execute();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ProductionStartup()
        {
            try
            {
                //Unique Username with Text type index while ascending/descending for SessionId
                DB.Index<Users>()
                    .Option(SetOption => SetOption.Unique = true)
                    .Option(SetOption => SetOption.Background = true)
                    .Key(User => User.Username, KeyType.Text)
                    .Key(User => User.SessionId, KeyType.Ascending)
                    .Create();

                //Logs index with Descending datetime
                DB.Index<Logs>()
                    .Option(SetOption => SetOption.Background = true)
                    .Key(Log => Log.DateTime, KeyType.Descending)
                    .Create();

                //Logs index with Descending datetime
                DB.Index<SupportTickets>()
                    .Option(SetOption => SetOption.Background = true)
                    .Key(Ticket => Ticket.DateTime, KeyType.Descending)
                    .Create();

                //Ensure our current settings exist in Settings collection
                //And insert if doesn't exist
                //I could not find a better method for "insert if not exist" so I split queries into 2 parts

                Guid SettingsExist = DB.Queryable<Settings>()
                    .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                    .Select(Setting => Setting.SettingsID)
                    .FirstOrDefault();
                if (!SettingsExist.Equals(KeysVariable.SettingsKey))
                {
                    Settings NewSetting = new Settings()
                    {
                        SettingsID = KeysVariable.SettingsKey
                    };
                    NewSetting.Save();
                }

                //Insert all banner networks if they don't already exist
                foreach (KeyValuePair<string, string[]> NetworkList in HTMLCodeVariable.AllNetworks)
                {
                    foreach (string Network in NetworkList.Value)
                    {
                        int MaxId = GetMaxId.Network(NetworkList.Key, 1);

                        DB.Update<Settings>()
                       .Match(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                       .Match(Setting => !Setting.BannerNetwork.Any(Element => Element.HTMLCode.Equals(Network) && Element.Type.Equals(NetworkList.Key)))
                       .Modify(Filter => Filter.Push(Setting => Setting.BannerNetwork, new BannerNetworkModel
                       {
                           Id = MaxId,
                           HTMLCode = Network,
                           Type = NetworkList.Key
                       }))
                       .Execute();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}