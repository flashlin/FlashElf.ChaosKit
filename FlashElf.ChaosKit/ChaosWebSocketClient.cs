using System;
using System.Threading;
using Microsoft.Extensions.Options;
using WebSocketSharp;

namespace FlashElf.ChaosKit
{
	public class ChaosWebSocketClient : IChaosClient
	{
		private readonly IOptions<ChaosClientConfig> _config;
		private WebSocket _client;
		private readonly ChaosBinarySerializer _binarySerializer;
		private AutoResetEvent _singal;
		private ChaosInvocationResp _reply;
		private readonly IChaosConverter _chaosConverter;
		private readonly object _lock = new object();

		public ChaosWebSocketClient(IOptions<ChaosClientConfig> config, IChaosConverter chaosConverter)
		{
			_chaosConverter = chaosConverter;
			_config = config;
			_binarySerializer = new ChaosBinarySerializer();
			Initialize();
		}

		public object Send(ChaosInvocation invocation)
		{
			var data = _binarySerializer.Serialize(invocation);

			object resp = null;
			lock (_lock)
			{
				_singal = new AutoResetEvent(false);
				_client.Send(data);

				if (!_singal.WaitOne(TimeSpan.FromSeconds(20)))
				{
					throw new TimeoutException();
				}

				resp = _chaosConverter.ToData(_reply);
			}

			return resp;
		}

		private string GetUrl()
		{
			var config = _config.Value;
			var schema = config.IsSsl ? "wss" : "ws";
			var ipPort = config.GetIpPort();
			return $"{schema}://{ipPort}/ChaosService";
		}

		private void Initialize()
		{
			_client = new WebSocket(GetUrl());
			_client.OnMessage += ClientOnOnMessage;
			_client.Connect();
		}

		private void ClientOnOnMessage(object sender, MessageEventArgs e)
		{
			_reply = (ChaosInvocationResp)_binarySerializer.Deserialize(typeof(ChaosInvocationResp), e.RawData);
			_singal.Set();
		}
	}
}