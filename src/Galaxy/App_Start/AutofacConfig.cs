using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Galaxy
{
	public class AutofacConfig
	{
		class AutofacModule : Module
		{
			protected override void Load(ContainerBuilder builder)
			{

			}
		}

		public static IServiceProvider ConfigureServices(IServiceCollection services)
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<AutofacModule>();
			builder.Populate(services);
			Container = builder.Build();

			return Container.Resolve<IServiceProvider>();
		}

		public static IContainer Container { get; private set; }
	}
}
