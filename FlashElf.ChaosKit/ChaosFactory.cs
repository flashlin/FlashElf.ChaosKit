﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using T1.Standard.DesignPatterns.Cache;
using T1.Standard.Extensions;

namespace FlashElf.ChaosKit
{
	public class ChaosFactory : IChaosFactory
	{
		private readonly IChaosSerializer _serializer;
		private readonly IChaosPromiseInvocationClient _chaosPromiseInvocationSubjects;

		public ChaosFactory(IChaosSerializer serializer, 
			IChaosPromiseInvocationClient chaosPromiseInvocationSubjects)
		{
			_chaosPromiseInvocationSubjects = chaosPromiseInvocationSubjects;
			_serializer = serializer;
		}

		public TService CreateChaosService<TService>()
			where TService : class
		{
			var chaosInterceptor = new ChaosInterceptor(typeof(TService),
				this, _chaosPromiseInvocationSubjects);
			return T1.Standard.CastleEx.Interceptor.InterceptInterface<TService>(chaosInterceptor);
		}

		public ChaosInvocation CreateChaosInvocation(Type implementType,
			MethodInfo invocationMethod,
			object[] invocationArguments)
		{
			var chaosInvocation = new ChaosInvocation()
			{
				InterfaceTypeFullName = implementType.FullName,
				MethodName = invocationMethod.Name,
				GenericTypes = GetGenericTypes(invocationMethod).ToArray(),
				Parameters = invocationMethod.GetChaosParameters(_serializer, invocationArguments)
			};

			if (invocationMethod.ReturnType != typeof(void))
			{
				chaosInvocation.ReturnTypeFullName = invocationMethod.ReturnType.FullName;
				if (invocationMethod.ReturnType.IsTaskType())
				{
					chaosInvocation.IsReturnTask = true;
					var resultType = invocationMethod.ReturnType.GenericTypeArguments[0];
					chaosInvocation.ReturnTypeFullName = resultType.FullName;
				}
			}

			return chaosInvocation;
		}

		private IEnumerable<ChaosParameter> GetGenericTypes(MethodInfo invocationMethod)
		{
			if (!invocationMethod.IsGenericMethod)
			{
				yield break;
			}

			foreach (var genericArgument in invocationMethod.GetGenericArguments())
			{
				yield return new ChaosParameter()
				{
					Name = genericArgument.Name,
					ParameterType = genericArgument.FullName
				};
			}
		}
	}
}
