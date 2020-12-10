using FlashElf.ChaosKit.Protos;
using Grpc.Core;

namespace FlashElf.ChaosKit
{
	public class ChaosServer : IChaosServer
	{
		private Server _server;
		private readonly IChaosServiceResolver _chaosServiceResolver;
		private readonly IChaosSerializer _serializer;
		private IChaosFactory _chaosFactory;

		public ChaosServer(IChaosServiceResolver chaosServiceResolver, IChaosSerializer serializer,
			IChaosFactory chaosFactory)
		{
			_chaosFactory = chaosFactory;
			_serializer = serializer;
			_chaosServiceResolver = chaosServiceResolver;
		}

		public ChaosServerOptions Options { get; set; } = new ChaosServerOptions();

		public void Start()
		{
			_server = new Server
			{
				Services =
				{
					ChaosProto.BindService(new ChaosServiceImpl(_chaosServiceResolver, _serializer))
				},
				Ports =
				{
					new ServerPort(Options.ListenIp, Options.ListenPort, ServerCredentials.Insecure)
				}
			};
			_server.Start();
		}

		public void Shutdown()
		{
			_server.ShutdownAsync().Wait();
		}
	}
}