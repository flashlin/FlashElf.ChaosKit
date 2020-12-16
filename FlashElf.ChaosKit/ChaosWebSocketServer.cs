
using System;
using System.Threading;
using System.Threading.Tasks;
using Fleck;

namespace FlashElf.ChaosKit
{
	public class ChaosWebSocketServer : IChaosServer
	{
		private WebSocketServer _server;
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
			_server = new WebSocketServer(GetUrl());
			_server.Start(MessageReceived);
		}

		private string GetUrl()
		{
			var schema = Options.IsSsl ? "wss" : "ws";
			return $"{schema}://{Options.ListenIp}:{Options.ListenPort}";
		}

		public void Shutdown()
		{
		}

		void MessageReceived(IWebSocketConnection webSocketConnection)
		{
			webSocketConnection.OnBinary = data =>
			{
				var req = (ChaosInvocation) _binarySerializer.Deserialize(typeof(ChaosInvocation), data);

				var resp = _chaosService.ProcessInvocation(req);

				var respData = _binarySerializer.Serialize(resp);

				webSocketConnection.Send(respData);
			};
		}
	}
}