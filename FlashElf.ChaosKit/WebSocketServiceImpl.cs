using WebSocketSharp;
using WebSocketSharp.Server;

namespace FlashElf.ChaosKit
{
	public class WebSocketServiceImpl : WebSocketBehavior
	{
		private readonly ChaosBinarySerializer _binarySerializer;
		private readonly IChaosService _chaosService;

		public WebSocketServiceImpl(IChaosService chaosService)
		{
			_chaosService = chaosService;
			_binarySerializer = new ChaosBinarySerializer();
		}
		protected override void OnMessage(MessageEventArgs e)
		{
			var req = (ChaosInvocation)_binarySerializer.Deserialize(typeof(ChaosInvocation), e.RawData);
			var resp = _chaosService.ProcessInvocation(req);
			var respData = _binarySerializer.Serialize(resp);
			Send(respData);
		}
	}
}