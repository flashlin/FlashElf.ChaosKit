using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using T1.Standard.Threads;

namespace FlashElf.ChaosKit
{
	public class ChaosWebSocketClient : IChaosClient
	{
		private readonly IOptions<ChaosClientConfig> _config;
		private WebSocketClient _client;
		private readonly ChaosBinarySerializer _binarySerializer;
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
			var data = _binarySerializer.Serialize(invocation);

			var resp = _client.Send(data);

			var reply = (ChaosInvocationResp)_binarySerializer.Deserialize(typeof(ChaosInvocationResp), resp);

			return _chaosConverter.ToData(reply);
		}

		private void Initialize()
		{
			var config = _config.Value;
			_client = new WebSocketClient();
			_client.Connect(new WebSocketClientOptions()
			{
				Ip = config.ChaosServerIp,
				Port = config.ChaosServerPort,
				IsSsl = false
			});
		}
	}
}