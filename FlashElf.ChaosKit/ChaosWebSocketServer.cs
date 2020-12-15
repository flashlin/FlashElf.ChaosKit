using WatsonWebsocket;

namespace FlashElf.ChaosKit
{
	public class ChaosWebSocketServer : IChaosServer
	{
		private WatsonWsServer _server;
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

		static void MessageReceived(object sender, MessageReceivedEventArgs args)
		{
			//Console.WriteLine("Message received from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
		}
	}
}