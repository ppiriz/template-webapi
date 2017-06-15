using Autofac;
using WebApi.template.Controllers;

namespace WebApi.template.Infrastructure.IoC
{
    /// <summary>
    /// Specific WebAPI <see cref="Autofac"/> module.
    ///     Modules can add a set of releated components to a container
    /// </summary>
    public class DefaultModule : Module
    {
        /// <summary>
        /// Add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BasicDependency>();
        }
    }
}