using FlashElf.ChaosKit.Protos;
using Grpc.Core;

namespace FlashElf.ChaosKit
{
	public class ChaosServer : IChaosServer
	{
		private Server _server;
		private readonly IChaosServiceResolver _chaosServiceResolver;
		private readonly IChaosSerializer _serializer;

		public ChaosServer(IChaosServiceResolver chaosServiceResolver, IChaosSerializer serializer)
		{
			_serializer = serializer;
			_chaosServiceResolver = chaosServiceResolver;
		}

		public int Port { get; set; } = 50050;

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
					new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure)
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