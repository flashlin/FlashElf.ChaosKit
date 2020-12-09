using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlashElf.ChaosKit.Protos;
using Google.Protobuf;

namespace FlashElf.ChaosKit
{
	public class ChaosFactory : IChaosFactory
	{
		private readonly string _chaosServer;
		private readonly IChaosSerializer _serializer;
		private readonly ChaosBinarySerializer _binarySerializer;

		public ChaosFactory(string chaosServer, IChaosSerializer serializer)
		{
			_binarySerializer = new ChaosBinarySerializer();
			_serializer = serializer;
			_chaosServer = chaosServer;
		}

		public TService CreateChaosService<TService>()
			where TService : class
		{
			var chaosInterceptor = new ChaosInterceptor(
				_chaosServer, 
				typeof(TService), 
				_serializer,
				this);
			return T1.Standard.CastleEx.Interceptor.InterceptInterface<TService>(chaosInterceptor);
		}

		public ChaosInvocation CreateChaosInvocation(Type implementType, 
			MethodInfo invocationMethod,
			object[] invocationArguments)
		{
			var chaosInvocation = new ChaosInvocation()
			{
				InterfaceName = implementType.FullName,
				MethodName = invocationMethod.Name,
				GenericTypes = GetGenericTypes(invocationMethod).ToArray(),
				Parameters = invocationMethod.GetChaosParameters(_serializer, invocationArguments)
			};

			if (invocationMethod.ReturnType != typeof(void))
			{
				chaosInvocation.ReturnTypeFullName = invocationMethod.ReturnType.FullName;
			}

			return chaosInvocation;
		}

		public ChaosRequest CreateChaosRequest(ChaosInvocation invocation)
		{
			return new ChaosRequest()
			{
				Invocation = ByteString.CopyFrom(_binarySerializer.Serialize(invocation))
			};
		}

		public ChaosInvocation GetChaosInvocationFrom(ChaosRequest request)
		{
			return (ChaosInvocation)_binarySerializer.Deserialize(typeof(ChaosInvocation), 
				request.Invocation.ToByteArray());
		}

		public ChaosReply CreateChaosReply(string returnTypeFullName, object returnValue)
		{
			var invocationReply = new ChaosInvocationResp()
			{
				DataTypeFullName = returnTypeFullName,
				Data = _serializer.Serialize(returnValue)
			};

			return new ChaosReply()
			{
				Data = ByteString.CopyFrom(_binarySerializer.Serialize(invocationReply))
			};
		}

		public ChaosInvocationResp GetInvocationResp(ChaosReply reply)
		{
			var byteArray = reply.Data.ToByteArray();
			return (ChaosInvocationResp)_binarySerializer.Deserialize(typeof(ChaosInvocationResp), byteArray);
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
