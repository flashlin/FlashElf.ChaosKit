using FlashElf.ChaosKit.Protos;

namespace FlashElf.ChaosKit
{
	public interface IChaosService
	{
		ChaosInvocationResp ProcessInvocation(ChaosInvocation chaosInvocation);
	}
}