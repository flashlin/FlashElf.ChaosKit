using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FlashElf.ChaosKit.Autofac
{
	public class AutofacStarter
	{
		private ContainerBuilder _containerBuilder;
		private ServiceCollection _serviceCollection;
		private AutofacServiceProvider _serviceProvider;

		public AutofacStarter(ContainerBuilder containerBuilder = null)
		{
			if (containerBuilder == null)
			{
				containerBuilder = new ContainerBuilder();
			}
			ConfigureContainerBuilder(containerBuilder);
		}

		public ServiceCollection ServiceCollection => _serviceCollection;
		public AutofacServiceProvider ServiceProvider => _serviceProvider;

		private void ConfigureContainerBuilder(ContainerBuilder containerBuilder)
		{
			_containerBuilder = containerBuilder;
			_serviceCollection = new ServiceCollection();
		}

		public IContainer Build()
		{
			_containerBuilder.Populate(_serviceCollection);
			var container = _containerBuilder.Build();
			_serviceProvider = new AutofacServiceProvider(container);
			return container;
		}

		public void AddChaosServices(string chaosServer)
		{
			_serviceCollection.AddChaosServices(chaosServer);
		}

		public void AddChaosTransient<T>()
			where T: class
		{
			_serviceCollection.AddChaosTransient<T>();
		}

		public T Resolve<T>()
		{
			return _serviceProvider.GetService<T>();
		}
	}
}