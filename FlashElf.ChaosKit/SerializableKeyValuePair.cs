using System;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class SerializableKeyValuePair
	{
		public byte[] Key { get; set; }
		public byte[] Value { get; set; }
	}
}