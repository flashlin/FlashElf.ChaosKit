namespace FlashElf.ChaosKit
{
	public interface IChaosFactory
	{
		TRepoInterface Create<TRepoInterface>()
			where TRepoInterface : class;
	}
}