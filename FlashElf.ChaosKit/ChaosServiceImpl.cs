using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FlashElf.ChaosKit.Protos;
using Google.Protobuf;
using Grpc.Core;
using T1.Standard.DynamicCode;
using T1.Standard.Serialization;

namespace FlashElf.ChaosKit
{
	public class ChaosServiceImpl : ChaosProto.ChaosProtoBase
	{
		private readonly IChaosServiceResolver _serviceResolver;
		private readonly IChaosSerializer _serializer;
		private readonly ChaosBinarySerializer _binarySerializer;

		public ChaosServiceImpl(IChaosServiceResolver serviceResolver, IChaosSerializer serializer)
		{
			_serializer = serializer;
			_serviceResolver = serviceResolver;
			_binarySerializer = new ChaosBinarySerializer();
		}

		public override Task<ChaosReply> Send(ChaosRequest request,
			ServerCallContext context)
		{
			var realImplementObject = _serviceResolver.GetService(request.InterfaceName);
			var realImplementType = realImplementObject.GetType();
			var realImplementInfo = ReflectionClass.Reflection(realImplementType);

			if (realImplementInfo.MethodsInfo.TryGetValue(request.MethodName, out var mi))
			{
				var parameters = (List<ChaosParameter>)_serializer.Deserialize(typeof(List<ChaosParameter>), request.Parameters.ToByteArray());
				var args = parameters.Select(x => x.Value).ToArray();
				var returnValue = mi.Func(realImplementObject, args);

				var invocationReply = new ChaosInvocationResp()
				{
					DataTypeFullName = mi.MethodInfo.ReturnType?.FullName,
					Data = _serializer.Serialize(returnValue)
				};

				var reply = new ChaosReply()
				{
					Data = ByteString.CopyFrom(_binarySerializer.Serialize(invocationReply))
				};

				return Task.FromResult(reply);
			}

			throw new NotImplementedException();
		}
	}
}