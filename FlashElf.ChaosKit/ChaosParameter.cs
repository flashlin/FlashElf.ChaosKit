using System;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class ChaosParameter
	{
		public string ParameterType { get; set; }
		public string Name { get; set; }
		public byte[] Value { get; set; }
	}
}