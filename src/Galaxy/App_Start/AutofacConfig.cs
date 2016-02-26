using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Galaxy.Infrastructure.Repositories.Abstract;
using Galaxy.Infrastructure.Repositories;
using Galaxy.Infrastructure.Services.Abstract;
using Galaxy.Infrastructure.Services;

namespace Galaxy
{
	public class AutofacConfig
	{
		class AutofacModule : Module
		{
			protected override void Load(ContainerBuilder builder)
			{
                builder.RegisterType<UserRepository>().As<IUserRepository>();
                builder.RegisterType<RoleRepository>().As<IRoleRepository>();
                builder.RegisterType<UserRoleRepository>().As<IUserRoleRepository>();
                
                builder.RegisterType<EncryptionService>().As<IEncryptionService>();
                builder.RegisterType<MembershipService>().As<IMemberShipService>();
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
