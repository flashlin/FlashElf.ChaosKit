using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using T1.Standard.Common;

namespace FlashElf.ChaosKit
{
	public class ChaosServiceResolver : IChaosServiceResolver
	{
		private readonly IServiceProvider _serviceProvider;

		public ChaosServiceResolver(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public object GetService(string interfaceTypename)
		{
			var interfaceType = new TypeFinder().Find(interfaceTypename);
			var services = _serviceProvider.GetServices(interfaceType);
			return services.FirstOrDefault();
		}
	}
}