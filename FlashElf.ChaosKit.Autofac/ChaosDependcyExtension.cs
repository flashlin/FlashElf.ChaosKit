using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace FlashElf.ChaosKit.Autofac
{
	public static class ChaosDependcyExtension
	{
		public static void AddChaosServices(this ContainerBuilder services, string chaosServer)
		{
			services.RegisterType<ChaosServiceResolver>().As<IChaosServiceResolver>();
			services.RegisterType<ChaosServer>().As<IChaosServer>();
			services.Register<IChaosFactory>(sp => new ChaosFactory(chaosServer));
		}

		public static void AddChaosTransient<TServiceType>(this ContainerBuilder services)
			where TServiceType : class
		{
			services.Register<TServiceType>(sp =>
				sp.Resolve<IChaosFactory>().Create<TServiceType>());
		}
	}
}
