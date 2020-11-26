using Microsoft.Extensions.DependencyInjection;

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
			services.AddTransient<TServiceType>(sp =>
				sp.GetService<IChaosFactory>().Create<TServiceType>());
		}
	}
}
