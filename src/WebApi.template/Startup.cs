using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevOpsFlex.Telemetry;
using DevOpsFlex.Telemetry.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.template
{
    /// <summary>
    /// ASP.NET Core main bootstrapping entry point.
    /// </summary>
    public class Startup
    {
        internal readonly BigBrother Bb;
        internal readonly TelemetrySettings TelemetrySettings = new TelemetrySettings();

        /// <summary>
        /// Gets the Configuration root for this ASP.NET Core application.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                                    .AddJsonFile("appsettings.json");

            if (!env.IsDevelopment())
            {
                builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json");
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();

            Configuration.GetSection("Telemetry").Bind(TelemetrySettings);
            Bb = new BigBrother(TelemetrySettings.InstrumentationKey, TelemetrySettings.InternalKey);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        /// <returns>The <see cref="Autofac"/> service object that provides Dependency Injection support to other objects.</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddMvc();
                services.AddMvc(options => options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.StringOutputFormatter>());

                services.AddApplicationInsightsTelemetry(TelemetrySettings.InstrumentationKey);

                services.AddSwaggerGen();
                services.ConfigureSwaggerGen(c =>
                {
                    var xmlDocFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "webApi.template.xml");

                    if (File.Exists(xmlDocFile))
                    {
                        c.IncludeXmlComments(xmlDocFile);
                    }

                    c.DescribeAllEnumsAsStrings(); // if this is not enabled, enum values are treated as int's. probably not what you want
                    c.SingleApiVersion(new Swashbuckle.Swagger.Model.Info { Title = "webApi.template", Version = "v1" });
                });

                var builder = new ContainerBuilder();
                builder.RegisterAssemblyModules(GetType().GetTypeInfo().Assembly); // autoscan the current assembly, find all modules and load them
                builder.Populate(services);
                builder.Register(c => Bb).As<IBigBrother>();

                return new AutofacServiceProvider(builder.Build());
            }
            catch (Exception e)
            {
                Bb.Publish(e.ToBbEvent());
                throw;
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure an application's request pipeline.
        /// </summary>
        /// <param name="app">Provides the mechanisms to configure an application's request pipeline.</param>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                app.UseBigBrotherExceptionHandler();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseMvcWithDefaultRoute();
                app.UseSwagger();

                if (env.IsDevelopment())
                {
                    app.UseSwaggerUi();
                }
            }
            catch (Exception e)
            {
                Bb.Publish(e.ToBbEvent());
                throw;
            }
        }
    }
}
