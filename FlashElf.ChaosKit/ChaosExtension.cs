using System;
using System.Linq;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using T1.Standard.DynamicCode;

namespace FlashElf.ChaosKit
{
	public static class ChaosExtension
	{
		public static void AddChaosServices(this IServiceCollection services, string chaosServer)
		{
			services.AddTransient<IChaosSerializer, ChaosSerializer>();
			services.AddTransient<IChaosServiceResolver, ChaosServiceResolver>();
			services.AddSingleton<IChaosServer, ChaosServer>();
			services.AddTransient<IChaosFactory>(sp => 
				new ChaosFactory(chaosServer, sp.GetService<IChaosSerializer>()));
		}

		public static void AddChaosTransient<TServiceType>(this IServiceCollection services)
			where TServiceType : class
		{
			var isExists = services
				.Any(i => i.ServiceType == typeof(TServiceType));

			if (isExists)
			{
				return;
			}

			services.AddTransient<TServiceType>(sp =>
				sp.GetService<IChaosFactory>().Create<TServiceType>());
		}

		public static void AddChaosInterfaces(this IServiceCollection services, Assembly assembly)
		{
			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				if( !type.IsInterface )	continue;
				var attr = type.GetCustomAttribute<ChaosInterfaceAttribute>();
				if( attr ==null) continue;

				var addChaosTransient = DynamicMethod.GetGenericMethod(typeof(ChaosExtension),
					new[] {type}, nameof(AddChaosTransient),
					new [] { typeof(IServiceCollection) });

				addChaosTransient(null, new object[]{ services });
			}
		}
	}
}
