using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FlashElf.ChaosKit.Autofac
{
	public class ChaosServiceProvider : IChaosServiceProvider
	{
		public ChaosServiceProvider(IContainer container)
		{
			ServiceProvider = new AutofacServiceProvider(container);
		}

		public AutofacServiceProvider ServiceProvider { get; set; }

		public T GetService<T>()
		{
			return ServiceProvider.GetService<T>();
		}
	}
}