namespace FlashElf.ChaosKit
{
	public interface IChaosServiceResolver
	{
		object GetService(string interfaceTypename);
	}
}