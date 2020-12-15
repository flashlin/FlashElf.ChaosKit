using System.Threading.Tasks;
using FlashElf.ChaosKit.Protos;
using Google.Protobuf;
using Grpc.Core;
using T1.Standard.Serialization;

namespace FlashElf.ChaosKit
{
	public class ChaosServiceImpl : ChaosProto.ChaosProtoBase
	{
		private readonly ChaosService _chaosService;

		public ChaosServiceImpl(IChaosServiceResolver serviceResolver, 
			IChaosSerializer serializer)
		{
			_chaosService = new ChaosService(serializer, serviceResolver);
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