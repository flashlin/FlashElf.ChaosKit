using System;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class JsonModelWrapper
	{
		public string ModelTypeFullName { get; set; }
		public string JsonValue { get; set; }
	}
}