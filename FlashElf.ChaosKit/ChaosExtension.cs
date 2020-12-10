﻿using System;
using System.Linq;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using T1.Standard.DynamicCode;
using Type = Google.Protobuf.WellKnownTypes.Type;

namespace FlashElf.ChaosKit
{
	public static class ChaosExtension
	{
		public static void AddChaosServices(this IServiceCollection services, string chaosServer)
		{
			services.TryAddTransient<IChaosSerializer, ChaosBinarySerializer>();
			services.TryAddTransient<IChaosServiceResolver, ChaosServiceResolver>();
			services.AddSingleton<IChaosServer, ChaosServer>();
			services.AddTransient<IChaosFactory>(sp =>
			{
				var chaosFactory = new ChaosFactory(chaosServer, sp.GetService<IChaosSerializer>());
				return new CachedChaosFactory(chaosFactory);
			});
		}

		public static void AddChaosTransient<TServiceType>(this IServiceCollection services)
			where TServiceType : class
		{
			var isExists = IsRegisted<TServiceType>(services);

			if (isExists)
			{
				return;
			}

			services.AddTransient<TServiceType>(sp =>
				sp.GetService<IChaosFactory>().CreateChaosService<TServiceType>());
		}

		public static void TryAddTransient<TServiceType, TImplementType>(this IServiceCollection services)
			where TServiceType : class
			where TImplementType : class, TServiceType
		{
			if (IsRegisted<TServiceType>(services))
			{
				return;
			}
			services.AddTransient<TServiceType, TImplementType>();
		}

		private static bool IsRegisted<TServiceType>(IServiceCollection services) where TServiceType : class
		{
			return services
				.Any(i => i.ServiceType == typeof(TServiceType));
		}

		public static void AddChaosInterfaces(this IServiceCollection services, Assembly assembly)
		{
			AddChaosInterfaces(services, assembly, type =>
			{
				var attr = type.GetCustomAttribute<ChaosInterfaceAttribute>();
				return attr != null;
			});
		}

		public static void AddChaosInterfaces(this IServiceCollection services, 
			Assembly assembly, 
			Func<System.Type, bool> predicate)
		{
			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				if (!type.IsInterface) continue;

				if (!predicate(type))
				{
					continue;
				}

				var addChaosTransient = DynamicMethod.GetGenericMethod(typeof(ChaosExtension),
					new[] { type }, nameof(AddChaosTransient),
					new[] { typeof(IServiceCollection) });

				addChaosTransient(null, new object[] { services });
			}
		}
	}
}
