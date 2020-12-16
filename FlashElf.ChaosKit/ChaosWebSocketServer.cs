using WatsonWebsocket;

namespace FlashElf.ChaosKit
{
	public class ChaosWebSocketServer : IChaosServer
	{
		private WatsonWsServer _server;
		private readonly ChaosBinarySerializer _binarySerializer;
		private readonly IChaosService _chaosService;

		public ChaosWebSocketServer(IChaosService chaosService)
		{
			_chaosService = chaosService;
			_binarySerializer = new ChaosBinarySerializer();
		}

		public ChaosServerOptions Options { get; set; } = new ChaosServerOptions();

		public void Start()
		{
			_server = new WatsonWsServer(Options.ListenIp, Options.ListenPort, false);
			_server.ClientConnected += ClientConnected;
			_server.ClientDisconnected += ClientDisconnected;
			_server.MessageReceived += MessageReceived;
			_server.Start();
		}

		public void Shutdown()
		{
			_server.Stop();
		}

		static void ClientConnected(object sender, ClientConnectedEventArgs args)
		{
			//Console.WriteLine("Client connected: " + args.IpPort);
		}

		static void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
		{
			//Console.WriteLine("Client disconnected: " + args.IpPort);
		}

		void MessageReceived(object sender, MessageReceivedEventArgs args)
		{
			var req = (ChaosInvocation) _binarySerializer.Deserialize(typeof(ChaosInvocation), args.Data);

			var resp = _chaosService.ProcessInvocation(req);

			var data = _binarySerializer.Serialize(resp);

			_server.SendAsync(args.IpPort, data);
		}
	}
}