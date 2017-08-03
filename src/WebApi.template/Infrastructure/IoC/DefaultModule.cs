using System;
using Autofac;
using Autofac.Features.ResolveAnything;
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

            // http://docs.autofac.org/en/latest/register/scanning.html

            // choose your convention...
            if(false)
            {
                // this will scan the current assembly, and map any interfaces found on classes to resolve to that class.
                // e.g. public class Foo : IFoo,IBar will automagically resolve BOTH IFoo and IBar to Foo
                builder.RegisterAssemblyTypes(GetType().Assembly)
                .AsImplementedInterfaces();
            }
            else
            {
                // similar to the above, but it will only map the interface with the same naming convention of the class.
                // e.g. public class Foo : IFoo,IBar will automagically resolve ONLY IFoo to Foo
                builder.RegisterAssemblyTypes(GetType().Assembly)
                    .Where(t => WithDefaultConvention(t) != null)
                    .As(svc => WithDefaultConvention(svc));
            }

            // this will resolve any concrete types as themselves
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
        }

        private static Type WithDefaultConvention(Type t) => t.GetInterface("I" + t.Name);
    }
}