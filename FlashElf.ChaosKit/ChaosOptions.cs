using System;
using Microsoft.Extensions.DependencyInjection;

namespace FlashElf.ChaosKit
{
	public class ChaosOptions
	{
		private readonly IServiceCollection _services;
		private bool _useProtocol;

		public ChaosOptions(IServiceCollection services)
		{
			_services = services;
			_useProtocol = false;
		}

		public ChaosClientConfig ClientConfig { get; set; } = new ChaosClientConfig();

		public void SetChaosServerIpPort(string ipPort)
		{
			var ss = ipPort.Split(':');
			var ip = ss[0];
			var port = ss[1];
			ClientConfig.ChaosServerIp = ip;
			ClientConfig.ChaosServerPort = Int32.Parse(port);
		}

		public void UseGrpc()
		{
			Use(() =>
			{
				_services.TryAddTransient<IChaosClient, ChaosGrpcClient>();
				_services.AddSingleton<IChaosServer, ChaosGrpcServer>();
			});
		}

		public void UseWebSocket()
		{
			Use(() =>
			{
				_services.TryAddTransient<IChaosClient, ChaosWebSocketClient>();
				_services.AddSingleton<IChaosServer, ChaosWebSocketServer>();
			});
		}

		private void Use(Action useAction)
		{
			if (_useProtocol)
			{
				return;
			}
			useAction();
			_useProtocol = true;
		}
	}
}