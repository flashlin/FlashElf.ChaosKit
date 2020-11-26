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
			_containerBuilder.Populate(_serviceCollection);
		}

		public IContainer Build(IContainer container = null)
		{
			if (container == null)
			{
				container = _containerBuilder.Build();
			}

			_serviceProvider = new AutofacServiceProvider(container);
			return container;
		}
	}
}