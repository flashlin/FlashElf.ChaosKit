using System;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class ChaosInvocationResp
	{
		public string DataTypeFullName { get; set; }
		public byte[] Data { get; set; }
		public SerializeException Exception { get; set; }
	}
}