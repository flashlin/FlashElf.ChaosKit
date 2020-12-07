using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using T1.Standard.Common;

namespace FlashElf.ChaosKit.Autofac
{
	public class ChaosServiceProvider : IChaosServiceResolver
	{
		private readonly TypeFinder _typeFinder;

		public ChaosServiceProvider(IContainer container)
		{
			ServiceProvider = new AutofacServiceProvider(container);
			_typeFinder = new TypeFinder();
		}

		public AutofacServiceProvider ServiceProvider { get; set; }

		public object GetService(string interfaceTypename)
		{
			return ServiceProvider.GetService(_typeFinder.Find(interfaceTypename));
		}
	}
}