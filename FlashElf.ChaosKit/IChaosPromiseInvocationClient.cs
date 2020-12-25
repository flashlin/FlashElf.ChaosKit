namespace FlashElf.ChaosKit
{
	public interface IChaosPromiseInvocationClient
	{
		object Call(ChaosInvocation chaosInvocation);
	}
}