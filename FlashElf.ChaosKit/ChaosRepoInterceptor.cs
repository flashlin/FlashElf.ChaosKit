using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace FlashElf.ChaosKit
{
	public class ChaosRepoInterceptor : IInterceptor
	{
		private readonly Type _repoInterfaceType;
		private readonly ChaosClient _chaosClient;

		public ChaosRepoInterceptor(string chaosServer, Type repoInterfaceType, IChaosSerializer serializer)
		{
			_repoInterfaceType = repoInterfaceType;
			_chaosClient = new ChaosClient(chaosServer, serializer);
		}

		public void Intercept(IInvocation invocation)
		{
			var chaInvocation = new ChaosRepoInvocation()
			{
				InterfaceName = _repoInterfaceType.FullName,
				MethodName = GetMethodName(invocation),
				Parameters = GetParameters(invocation)
			};

			if (invocation.Method.ReturnType != typeof(void))
			{
				invocation.ReturnValue = _chaosClient.Send(chaInvocation);
			}
		}

		private static string GetMethodName(IInvocation invocation)
		{
			return invocation.Method.Name;
		}

		private static List<ChaosParameter> GetParameters(IInvocation invocation)
		{
			var args = new List<ChaosParameter>();
			var parameters = invocation.Method.GetParameters();
			for (var i = 0; i < invocation.Arguments.Length; i++)
			{
				var parameter = new ChaosParameter()
				{
					Name = parameters[i].Name,
					Value = invocation.Arguments[i]
				};
				args.Add(parameter);
			}
			return args;
		}
	}
}