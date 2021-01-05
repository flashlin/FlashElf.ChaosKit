using System;

namespace FlashElf.ChaosKit
{
	public class ChaosClientConfig
	{
		public string ChaosServerIp { get; set; } = "127.0.0.1";
		public int ChaosServerPort { get; set; } = 50050;
		public bool IsSsl { get; set; } = false;
		public TimeSpan WaitTimeout { get; set; } = TimeSpan.FromSeconds(60);

		public string GetIpPort()
		{
			return $"{ChaosServerIp}:{ChaosServerPort}";
		}
	}
}