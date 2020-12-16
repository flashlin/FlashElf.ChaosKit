using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using T1.Standard.Threads;

namespace FlashElf.ChaosKit
{
	public class WebSocketClient
	{
		private ClientWebSocket _cli;
		private CancellationTokenSource _cancel;
		private readonly byte[] _receiveBuffer = new byte[1024 * 10];

		public WebSocketClient()
		{
		}

		public void Connect(WebSocketClientOptions options)
		{
			_cli = new ClientWebSocket();
			_cancel = new CancellationTokenSource();
			_cli.ConnectAsync(new Uri(options.GetUrl()), _cancel.Token);
		}

		public void Close()
		{
			_cancel.Cancel();
		}

		public byte[] Send(byte[] data)
		{
			_cli.SendAsync(new ArraySegment<byte>(data),
				WebSocketMessageType.Binary, 
				true, 
				_cancel.Token);

			return AsyncHelper.RunSync(async () => await ReceiveAsync());
		}

		public async Task<byte[]> ReceiveAsync()
		{
			var offset = 0;
			while (true)
			{
				var bytesReceived = new ArraySegment<byte>(_receiveBuffer, offset, _receiveBuffer.Length - offset);
				var result = await _cli.ReceiveAsync(bytesReceived, _cancel.Token);
				offset += result.Count;
				if (result.EndOfMessage)
				{
					break;
				}
			}

			var data = new byte[offset];
			Array.Copy(_receiveBuffer, data, offset);
			return data;
		}
	}
}