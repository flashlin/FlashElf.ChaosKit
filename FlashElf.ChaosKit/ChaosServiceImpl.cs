using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FlashElf.ChaosKit.Protos;
using Google.Protobuf;
using Grpc.Core;
using T1.Standard.Common;
using T1.Standard.DynamicCode;
using T1.Standard.Serialization;

namespace FlashElf.ChaosKit
{
	public class ChaosServiceImpl : ChaosProto.ChaosProtoBase
	{
		private readonly IChaosServiceResolver _serviceResolver;
		private readonly IChaosSerializer _serializer;
		private readonly ChaosBinarySerializer _binarySerializer;
		private readonly TypeFinder _typeFinder;
		private readonly IChaosFactory _chaosFactory;

		public ChaosServiceImpl(IChaosServiceResolver serviceResolver, 
			IChaosSerializer serializer,
			IChaosFactory chaosFactory)
		{
			_chaosFactory = chaosFactory;
			_serializer = serializer;
			_serviceResolver = serviceResolver;
			_binarySerializer = new ChaosBinarySerializer();
			_typeFinder = new TypeFinder();
		}

		public override Task<ChaosReply> Send(
			ChaosRequest request,
			ServerCallContext context)
		{
			var chaosInvocation = _chaosFactory.GetChaosInvocationFrom(request);
			var realImplementObject = _serviceResolver.GetService(chaosInvocation.InterfaceName);
			var realImplementType = realImplementObject.GetType();
			var realImplementInfo = ReflectionClass.Reflection(realImplementType);

			var requestParameters = chaosInvocation.Parameters;

			var mi = FindMethod(realImplementInfo, chaosInvocation);

			var args = requestParameters.DeserializeToArguments(_serializer, _typeFinder)
				.ToArray();

			var returnValue = mi.Func(realImplementObject, args);

			var reply = _chaosFactory.CreateChaosReply(chaosInvocation.ReturnTypeFullName, returnValue);
			return Task.FromResult(reply);
		}

		private (MethodInfo MethodInfo, Func<object, object[], object> Func) FindMethod(ReflectionClass clazz,
			ChaosInvocation request)
		{
			foreach (var method in clazz.AllMethods)
			{
				if (method.MethodInfo.Name != request.MethodName)
				{
					continue;
				}

				var parameterInfos = method.MethodInfo.GetParameters();
				if (parameterInfos.Length != request.Parameters.Count)
				{
					continue;
				}

				var isAllSame = parameterInfos.Zip(request.Parameters.Select(x => _typeFinder.Find(x.ParameterType)),
						(x, y) => x.ParameterType == y)
					.All(x => x == true);

				if (!isAllSame)
				{
					continue;
				}

				return (method.MethodInfo, method.Func);
			}

			foreach (var method in clazz.AllGenericMethods)
			{
				if (method.MethodInfo.Name != request.MethodName)
				{
					continue;
				}

				var parameterInfos = method.MethodInfo.GetParameters();
				if (parameterInfos.Length != request.Parameters.Count)
				{
					continue;
				}

				var isAllSame = parameterInfos.Zip(request.Parameters.Select(x => _typeFinder.Find(x.ParameterType)),
						(x, y) => x.ParameterType == y)
					.All(x => x == true);

				if (!isAllSame)
				{
					continue;
				}

				var genericTypes= request.GenericTypes
					.Select(x => _typeFinder.Find(x.ParameterType))
					.ToArray();

				return (method.MethodInfo, method.GetFunc(genericTypes));
			}

			throw new EntryPointNotFoundException(request.MethodName);
		}
	}
}