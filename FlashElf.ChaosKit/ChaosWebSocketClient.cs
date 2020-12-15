using System;
using System.Threading;
using Microsoft.Extensions.Options;
using T1.Standard.Threads;
using WatsonWebsocket;

namespace FlashElf.ChaosKit
{
	public class ChaosWebSocketClient : IChaosClient
	{
		private readonly IOptions<ChaosClientConfig> _config;
		private WatsonWsClient _client;
		private readonly ChaosBinarySerializer _binarySerializer;
		private AutoResetEvent _signal;
		private ChaosInvocationResp _resp;
		private readonly IChaosConverter _chaosConverter;

		public ChaosWebSocketClient(IOptions<ChaosClientConfig> config,
			IChaosConverter chaosConverter)
		{
			_chaosConverter = chaosConverter;
			_binarySerializer = new ChaosBinarySerializer();
			_config = config;
			Initialize();
		}

		public object Send(ChaosInvocation invocation)
		{
			_signal = new AutoResetEvent(false);
			var data = _binarySerializer.Serialize(invocation); 
			_client.SendAsync(data);
			if (!_signal.WaitOne(TimeSpan.FromSeconds(30)))
			{
				throw new TimeoutException();
			}

			return _chaosConverter.ToData(_resp);
		}

		private void Initialize()
		{
			var config = _config.Value;
			_client = new WatsonWsClient(config.ChaosServerIp, config.ChaosServerPort, false);
			_client.ServerConnected += ServerConnected;
			_client.ServerDisconnected += ServerDisconnected;
			_client.MessageReceived += MessageReceived;
			_client.Start();
		}

		void MessageReceived(object sender, MessageReceivedEventArgs args)
		{
			_resp = (ChaosInvocationResp)_binarySerializer.Deserialize(typeof(ChaosInvocationResp), args.Data);
			//Console.WriteLine("Message from server: " + Encoding.UTF8.GetString(args.Data));
		}

		static void ServerConnected(object sender, EventArgs args)
		{
			//Console.WriteLine("Server connected");
		}

		static void ServerDisconnected(object sender, EventArgs args)
		{
			//Console.WriteLine("Server disconnected");
		}
	}
}