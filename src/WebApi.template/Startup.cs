using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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
                                                    .AddJsonFile("appsettings.json")
                                                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                                                    .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        /// <returns>The <see cref="Autofac"/> service object that provides Dependency Injection support to other objects.</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMvc(options => options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.StringOutputFormatter>());

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

            return new AutofacServiceProvider(builder.Build());
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure an application's request pipeline.
        /// </summary>
        /// <param name="app">Provides the mechanisms to configure an application's request pipeline.</param>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
    }
}
