namespace FlashElf.ChaosKit.Autofac
{
	public interface IChaosServiceProvider
	{
		T GetService<T>();
	}
}