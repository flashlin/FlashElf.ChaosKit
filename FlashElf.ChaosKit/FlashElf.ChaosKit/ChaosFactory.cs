namespace FlashElf.ChaosKit
{
	public class ChaosFactory : IChaosFactory
	{
		private readonly string _chaosServer;

		public ChaosFactory(string chaosServer)
		{
			_chaosServer = chaosServer;
		}

		public TRepoInterface Create<TRepoInterface>()
			where TRepoInterface : class
		{
			var chaRepoInterceptor = new ChaosRepoInterceptor(_chaosServer, typeof(TRepoInterface));
			return T1.Standard.CastleEx.Interceptor.InterceptInterface<TRepoInterface>(chaRepoInterceptor);
		}
	}
}
