using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ChaosAspNetSiteSample.Models.Services;
using FlashElf.ChaosKit;
using FlashElf.ChaosKit.Autofac;

namespace ChaosAspNetSiteSample.App_Start
{
	public static class AutofacConfig
	{
		public static void Startup()
		{
			var builder = new ContainerBuilder();
			var autofacStarter = new AutofacStarter(builder);

			builder.RegisterControllers(Assembly.GetExecutingAssembly());
			builder.RegisterType<MyRepo>().As<IMyRepo>();

			builder.AddChaosServices("127.0.0.1:50050");
			builder.AddChaosTransient<IMyRepo>();

			var container = autofacStarter.Build();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			container.Resolve<IChaosServer>().Start();
		}
	}
}