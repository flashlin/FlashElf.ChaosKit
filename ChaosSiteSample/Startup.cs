using ChaosSiteSample.Models.Services;
using FlashElf.ChaosKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using T1.Standard.ServiceCollectionEx.Decoration;

namespace ChaosSiteSample
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.AddTransient<IMyRepo, MyRepo>();
			services.AddTransient<IDecorateRepo, MyFirstRepo>();
			services.Decorate<IDecorateRepo, MyFirstDecorateRepo>();
			services.AddSingleton<ISingletonRepo, MySingletonRepo>();

			services.AddChaosServices(options =>
			{
				options.SetChaosServerIpPort("127.0.0.1:50050");
				//options.UseWebSocket();
				options.UseGrpc();
			});

			services.AddChaosTransient<IMyRepo>();
			services.AddChaosTransient<IDecorateRepo>();
			services.AddChaosTransient<ISingletonRepo>();
			//services.AddChaosInterfaces(typeof(IMyRepo).Assembly);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IChaosServer chaosServer)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
						 name: "default",
						 pattern: "{controller=Home}/{action=Index}/{id?}");
			});


			chaosServer.Start();
		}
	}
}
