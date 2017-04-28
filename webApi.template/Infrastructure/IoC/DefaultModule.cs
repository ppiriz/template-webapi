using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using webApi.template.Controllers;

namespace webApi.template.Infrastructure.IoC
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BasicDependency>().As<IBasicDependency>();
        }
    }
}