using System;
using System.Collections.Generic;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class SerializableDictionary
	{
		public string KeyTypeFullName { get; set; }
		public string ValueTypeFullName { get; set; }
		public List<SerializableKeyValuePair> Items { get; set; } = new List<SerializableKeyValuePair>();
	}
}