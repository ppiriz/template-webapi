using Autofac;

namespace WebApi.template.Infrastructure.IoC
{
    using Controllers;

    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BasicDependency>().As<IBasicDependency>();
        }
    }
}