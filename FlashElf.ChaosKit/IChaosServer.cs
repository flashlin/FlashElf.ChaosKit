namespace FlashElf.ChaosKit
{
	public interface IChaosServer
	{
		void Start();
		void Shutdown();
		ChaosServerOptions Options { get; set; }
	}
}