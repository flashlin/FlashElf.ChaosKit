using FlashElf.ChaosKit.Protos;
using Grpc.Core;

namespace FlashElf.ChaosKit
{
	public class ChaosGrpcServer : IChaosServer
	{
		private Server _server;
		private readonly IChaosServiceResolver _chaosServiceResolver;
		private readonly IChaosSerializer _serializer;
		private readonly IChaosService _chaosService;

		public ChaosGrpcServer(IChaosServiceResolver chaosServiceResolver, 
			IChaosSerializer serializer,
			IChaosService chaosService)
		{
			_chaosService = chaosService;
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
					ChaosProto.BindService(new ChaosGrpcServiceImpl(_chaosServiceResolver, 
						_serializer, 
						_chaosService))
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