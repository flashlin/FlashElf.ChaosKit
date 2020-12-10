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
		private readonly ChaosClient _chaosClient;
		private readonly IChaosFactory _chaosFactory;

		public ChaosInterceptor(string chaosServer, Type implementType, 
			IChaosSerializer serializer, IChaosFactory chaosFactory)
		{
			_chaosFactory = chaosFactory;
			_implementType = implementType;
			_chaosClient = new ChaosClient(chaosServer, serializer);
		}

		public void Intercept(IInvocation invocation)
		{
			var chaosInvocation = _chaosFactory.CreateChaosInvocation(_implementType, invocation.Method, invocation.Arguments);
			invocation.ReturnValue = _chaosClient.Send(chaosInvocation);
		}
	}
}