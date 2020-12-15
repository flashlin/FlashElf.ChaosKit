using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace FlashElf.ChaosKit
{
	public class ChaosInterceptor : IInterceptor
	{
		private readonly Type _implementType;
		private readonly IChaosClient _chaosClient;
		private readonly IChaosFactory _chaosFactory;

		public ChaosInterceptor(Type implementType, IChaosFactory chaosFactory, IChaosClient chaosClient)
		{
			_chaosFactory = chaosFactory;
			_implementType = implementType;
			_chaosClient = chaosClient;
		}

		public void Intercept(IInvocation invocation)
		{
			var chaosInvocation = _chaosFactory.CreateChaosInvocation(_implementType, invocation.Method, invocation.Arguments);
			invocation.ReturnValue = _chaosClient.Send(chaosInvocation);
		}
	}
}