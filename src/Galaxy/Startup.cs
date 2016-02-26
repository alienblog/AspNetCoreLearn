using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Galaxy.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;

namespace Galaxy
{
	public class Startup
	{
		private static string _applicationPath = string.Empty;

		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
			_applicationPath = appEnv.ApplicationBasePath;

			var builder = new ConfigurationBuilder()
				.SetBasePath(appEnv.ApplicationBasePath)
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets();
			}
			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{

			//services.AddEntityFramework()
			//	.AddSqlServer()
			//	.AddDbContext<GalaxyContext>(options =>
			//		options.UseSqlServer(Configuration["Data:GalaxyConnection:ConnectionString"]));
			services.AddEntityFramework()
				.AddInMemoryDatabase()
				.AddDbContext<GalaxyContext>();

			services.AddAuthentication();

			services.AddAuthorization(AuthConfig.Config);

			services.AddMvc();

			return AutofacConfig.ConfigureServices(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole();
			loggerFactory.AddDebug();

			app.UseIISPlatformHandler();

			app.UseStaticFiles();

			AutoMapperConfig.Configure();

			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AutomaticAuthenticate = true,
				AutomaticChallenge = false
			});

			app.UseMvc(RouteConfig.RegisterRoute);
		}

		// Entry point for the application.
		public static void Main(string[] args)
		{
			var host = new WebHostBuilder()
				.UseDefaultConfiguration(args)
				.UseIISPlatformHandlerUrl()
				.UseServer("Microsoft.AspNetCore.Server.Kestrel")
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}
	}
}
