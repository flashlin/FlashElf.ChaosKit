using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace FlashElf.ChaosKit
{
	public class CachedChaosFactory : IChaosFactory
	{
		private readonly IChaosFactory _next;
		private static ConcurrentDictionary<Type, object> _services = new ConcurrentDictionary<Type, object>();
		public CachedChaosFactory(IChaosFactory next)
		{
			_next = next;
		}

		public TService CreateChaosService<TService>() 
			where TService : class
		{
			object AddValueFactory(Type serviceType) => _next.CreateChaosService<TService>();
			object UpdateValueFactory(Type serviceType, object service) => service;
			return (TService) _services.AddOrUpdate(typeof(TService), AddValueFactory, UpdateValueFactory);
		}

		public ChaosInvocation CreateChaosInvocation(Type implementType, 
			MethodInfo invocationMethod, 
			object[] invocationArguments)
		{
			return _next.CreateChaosInvocation(implementType, invocationMethod, invocationArguments);
		}
	}
}