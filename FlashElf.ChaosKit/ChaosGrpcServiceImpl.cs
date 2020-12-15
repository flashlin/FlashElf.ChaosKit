using System.Threading.Tasks;
using FlashElf.ChaosKit.Protos;
using Google.Protobuf;
using Grpc.Core;
using T1.Standard.Serialization;

namespace FlashElf.ChaosKit
{
	public class ChaosGrpcServiceImpl : ChaosProto.ChaosProtoBase
	{
		private readonly IChaosService _chaosService;

		public ChaosGrpcServiceImpl(IChaosServiceResolver serviceResolver, 
			IChaosSerializer serializer, IChaosService chaosService)
		{
			_chaosService = chaosService;
		}

		public override Task<AnyProto> SendInvocation(
			AnyProto request,
			ServerCallContext context)
		{
			var chaosInvocation = request.ConvertTo<ChaosInvocation>();
			var invocationReply = _chaosService.ProcessInvocation(chaosInvocation);
			var reply = invocationReply.ToAnyProto();
			return Task.FromResult(reply);
		}
	}
}