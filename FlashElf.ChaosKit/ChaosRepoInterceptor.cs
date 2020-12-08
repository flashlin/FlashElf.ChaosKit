using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace FlashElf.ChaosKit
{
	public class ChaosRepoInterceptor : IInterceptor
	{
		private readonly Type _implementType;
		private readonly ChaosClient _chaosClient;
		private readonly IChaosSerializer _serializer;

		public ChaosRepoInterceptor(string chaosServer, Type implementType, 
			IChaosSerializer serializer)
		{
			_serializer = serializer;
			_implementType = implementType;
			_chaosClient = new ChaosClient(chaosServer, serializer);
		}

		public void Intercept(IInvocation invocation)
		{
			var chaosInvocation = new ChaosInvocation()
			{
				InterfaceName = _implementType.FullName,
				MethodName = GetMethodName(invocation),
				GenericTypes = GetGenericTypes(invocation).ToArray(),
				Parameters = GetParameters(invocation)
			};

			if (invocation.Method.ReturnType != typeof(void))
			{
				invocation.ReturnValue = _chaosClient.Send(chaosInvocation);
			}
		}

		private IEnumerable<ChaosParameter> GetGenericTypes(IInvocation invocation)
		{
			if (!invocation.Method.IsGenericMethod)
			{
				yield break;
			}

			foreach (var genericArgument in invocation.Method.GetGenericArguments())
			{
				yield return new ChaosParameter()
				{
					Name = genericArgument.Name,
					ParameterType = genericArgument.FullName
				};
			}
			
		}

		private static string GetMethodName(IInvocation invocation)
		{
			return invocation.Method.Name;
		}

		private List<ChaosParameter> GetParameters(IInvocation invocation)
		{
			var args = new List<ChaosParameter>();
			var parameters = invocation.Method.GetParameters();
			for (var i = 0; i < invocation.Arguments.Length; i++)
			{
				var parameter = new ChaosParameter()
				{
					ParameterType = parameters[i].ParameterType.FullName,
					Name = parameters[i].Name,
					Value = _serializer.Serialize(invocation.Arguments[i])
				};
				args.Add(parameter);
			}
			return args;
		}
	}
}