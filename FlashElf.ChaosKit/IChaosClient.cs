namespace FlashElf.ChaosKit
{
	public interface IChaosClient
	{
		object Send(ChaosInvocation invocation);
	}
}