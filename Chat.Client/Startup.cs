using AspNetCore.Identity.MongoDB;
using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.Configuration.Settings;
using Chat.Infrastructure.Model;
using Chat.Repository.Implementation;
using Chat.Repository.Interface;
using Chat.Repository.Mongo.Implementation;
using Chat.Service.Consumers.Implementation;
using Chat.Service.Consumers.Interface;
using Chat.Service.Services.Implementation;
using Chat.Service.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OIA.Common.Repository.Mongo.Interface;
using RabbitMQ.Client;
using System;
using System.IO;

namespace Chat.Client
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var confBuilder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env.EnvironmentName }.json", optional: true)
				.AddEnvironmentVariables();

			Configuration = confBuilder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc()
				.AddCookieTempDataProvider();

			services.AddSession(options =>
			{
			});

			services.Configure<AMQPSettings>(Configuration.GetSection("AMQPSettings"));
			services.Configure<RabbitMQSettings>(Configuration.GetSection("RabbitMQSettings"));
			services.Configure<StompSettings>(Configuration.GetSection("StompSettings"));
			services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));

			//ASPNET Core Identity + MongoDB Setup
			services.Configure<MongoDBOption>(Configuration.GetSection("MongoDbIdentitySettings"))
				.AddMongoDatabase()
				.AddMongoDbContext<ApplicationUser, MongoIdentityRole>()
				.AddMongoStore<ApplicationUser, MongoIdentityRole>();

			services.AddIdentity<ApplicationUser, MongoIdentityRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredUniqueChars = 0;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;

				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
				options.Lockout.MaxFailedAccessAttempts = 10;
				options.Lockout.AllowedForNewUsers = true;

				options.User.RequireUniqueEmail = true;

			}).AddDefaultTokenProviders();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

			// Cookie settings
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.Cookie.Expiration = TimeSpan.FromDays(150);
				options.LoginPath = "/Auth/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
				options.LogoutPath = "/Auth/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
				options.AccessDeniedPath = "/Auth/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
				options.SlidingExpiration = true;
			});

			//SERVICE REGISTRATION

			services.AddTransient<IMongoChatDbContext,MongoChatDbContext>();
			services.AddTransient<IMongoUserDbContext, MongoUserDbContext>();

			services.AddSingleton<ConnectionFactory>(factory =>
			{
				var rabbitSettings = Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();
				return new ConnectionFactory
				{
					HostName = rabbitSettings.HostUrl,
					UserName = rabbitSettings.Username,
					Password = rabbitSettings.Password
				};
			}).AddSingleton<RabbitMQ.Client.IConnection>(factory => factory.GetService<ConnectionFactory>().CreateConnection());

			services.AddSingleton<IConfigurationManager, ConfigurationManager>();

			services.AddSingleton<IChatRoomRepository, ChatRoomRepository>();
			services.AddSingleton<IChatMessageRepository, ChatMessageRepository>();
			services.AddSingleton<IUserRepository, UserRepository>();

			services.AddSingleton<IChatRoomService, ChatRoomService>();
			services.AddSingleton<IChatMessageService, ChatMessageService>();
			services.AddSingleton<IUserService, UserService>();

			services.AddSingleton<IMessagePersistor, MessagePersistor>();
			services.AddSingleton<IStatusUpdatePersistor, StatusUpdatePersistor>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpContextAccessor context)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
				{
					HotModuleReplacement = true
				});
			}
			else
				app.UseExceptionHandler("/Home/Error");

			app.UseSession();

			app.UseAuthentication();

			app.UseStaticFiles();

			app.ApplicationServices.GetRequiredService<IMessagePersistor>();
			app.ApplicationServices.GetRequiredService<IStatusUpdatePersistor>();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				routes.MapSpaFallbackRoute(
					name: "spa-fallback",
					defaults: new { controller = "Home", action = "Index" });
			});
		}
	}
}
