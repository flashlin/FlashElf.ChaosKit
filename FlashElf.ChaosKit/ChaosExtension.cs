using System;
using System.Linq;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using T1.Standard.DynamicCode;
using Type = Google.Protobuf.WellKnownTypes.Type;

namespace FlashElf.ChaosKit
{
	public static class ChaosExtension
	{
		public static void AddChaosServices(this IServiceCollection services, string chaosServer)
		{
			services.TryAddIOptionsTransient(sp => new ChaosClientConfig()
			{
				ChaosServerIp = chaosServer
			});
			services.TryAddTransient<IChaosClient, ChaosClient>();
			services.TryAddTransient<IChaosSerializer, ChaosBinarySerializer>();
			services.TryAddTransient<IChaosServiceResolver, ChaosServiceResolver>();
			services.AddTransient<IChaosConverter, ChaosConverter>();
			services.AddSingleton<IChaosServer, ChaosGrpcServer>();
			services.AddTransient<IChaosFactory>(sp =>
			{
				var chaosFactory = new ChaosFactory(sp.GetService<IChaosSerializer>(), 
					sp.GetService<IChaosClient>());
				return new CachedChaosFactory(chaosFactory);
			});
		}

		private static void TryAddIOptionsTransient<TOptions>(this IServiceCollection services,
			Func<IServiceProvider, TOptions> create)
			where TOptions : class, new()
		{
			services.TryAddTransient<IOptions<TOptions>>(sp => 
				Options.Create(create(sp)));
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

		public static void TryAddTransient<TServiceType>(this IServiceCollection services,
			Func<IServiceProvider, TServiceType> create)
			where TServiceType : class
		{
			if (IsRegisted<TServiceType>(services))
			{
				return;
			}
			services.AddTransient<TServiceType>(create);
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
