using System;
using System.Collections.Generic;
using System.Linq;
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

		public ChaosServiceImpl(IChaosServiceResolver serviceResolver, IChaosSerializer serializer)
		{
			_serializer = serializer;
			_serviceResolver = serviceResolver;
		}

		public override Task<ChaosReply> Send(ChaosRequest request,
			ServerCallContext context)
		{
			var realRepo = _serviceResolver.GetService(request.InterfaceName);
			var realRepoType = realRepo.GetType();
			var realRepoInfo = ReflectionClass.Reflection(realRepoType);

			if (realRepoInfo.Methods.TryGetValue(request.MethodName, out var method))
			{
				var parameters = (List<ChaosParameter>)_serializer.Deserialize(typeof(List<ChaosParameter>), request.Parameters.ToByteArray());
				var args = parameters.Select(x => x.Value).ToArray();
				var returnValue = method(realRepo, args);

				var invocationReply = new ChaosRepoInvocationResp()
				{
					Data = returnValue
				};

				var reply = new ChaosReply()
				{
					Data = ByteString.CopyFrom(_serializer.Serialize(invocationReply))
				};

				return Task.FromResult(reply);
			}

			throw new NotImplementedException();
		}
	}
}