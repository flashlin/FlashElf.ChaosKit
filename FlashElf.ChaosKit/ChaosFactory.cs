namespace FlashElf.ChaosKit
{
	public class ChaosFactory : IChaosFactory
	{
		private readonly string _chaosServer;
		private IChaosSerializer _serializer;

		public ChaosFactory(string chaosServer, IChaosSerializer serializer)
		{
			_serializer = serializer;
			_chaosServer = chaosServer;
		}

		public TRepoInterface Create<TRepoInterface>()
			where TRepoInterface : class
		{
			var chaRepoInterceptor = new ChaosRepoInterceptor(_chaosServer, typeof(TRepoInterface), _serializer);
			return T1.Standard.CastleEx.Interceptor.InterceptInterface<TRepoInterface>(chaRepoInterceptor);
		}
	}
}
