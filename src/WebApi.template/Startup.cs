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
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            Configuration = Infrastructure.CoreConfiguration.Build();

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

                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1.0", new Info { Version = "v1.0", Title = "Shipping API V1.0" });

                    var xmlDocFile = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "WebApi.template.xml");

                    if (System.IO.File.Exists(xmlDocFile))
                    {
                        c.IncludeXmlComments(xmlDocFile);
                    }

                    c.DescribeAllEnumsAsStrings();

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
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "WebApi.template API V1.0");
                    });
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
