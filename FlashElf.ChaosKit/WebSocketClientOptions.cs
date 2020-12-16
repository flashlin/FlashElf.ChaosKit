namespace FlashElf.ChaosKit
{
	public class WebSocketClientOptions
	{
		public string Ip { get; set; }
		public int Port { get; set; }
		public bool IsSsl { get; set; }

		public string GetUrl()
		{
			var schema = IsSsl ? "wss" : "ws";
			return $"{schema}://{Ip}:{Port}";
		}
	}
}