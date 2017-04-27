using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using new_project.Controllers;

namespace new_project.Infrastructure.IoC
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BasicDependency>().As<IBasicDependency>();
        }
    }
}