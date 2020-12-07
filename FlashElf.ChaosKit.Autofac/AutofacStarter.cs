using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FlashElf.ChaosKit.Autofac
{
	public class AutofacStarter
	{
		private ContainerBuilder _containerBuilder;
		private ServiceCollection _serviceCollection;

		public AutofacStarter(ContainerBuilder containerBuilder = null)
		{
			if (containerBuilder == null)
			{
				containerBuilder = new ContainerBuilder();
			}
			ConfigureContainerBuilder(containerBuilder);
		}

		public ServiceCollection ServiceCollection => _serviceCollection;
		public IChaosServiceProvider ServiceProvider { get; set; }

		private void ConfigureContainerBuilder(ContainerBuilder containerBuilder)
		{
			_containerBuilder = containerBuilder;
			_serviceCollection = new ServiceCollection();
		}

		public IContainer Build()
		{
			_containerBuilder.Populate(_serviceCollection);
			var container = _containerBuilder.Build();
			ServiceProvider = new ChaosServiceProvider(container);
			return container;
		}

		public void AddChaosServices(string chaosServer)
		{
			_serviceCollection.AddChaosServices(chaosServer);
		}

		public void AddChaosTransient<T>()
			where T : class
		{
			_serviceCollection.AddChaosTransient<T>();
		}

		public void AddChaosInterfaces(Assembly assembly)
		{
			_serviceCollection.AddChaosInterfaces(assembly);
		}

		public T Resolve<T>()
		{
			return ServiceProvider.GetService<T>();
		}

		public void AddTransient<TService, TImplement>()
			where TService : class
			where TImplement : class, TService
		{
			_serviceCollection.AddTransient<TService, TImplement>();
		}
	}
}