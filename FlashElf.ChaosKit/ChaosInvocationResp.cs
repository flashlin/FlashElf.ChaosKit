using System;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class ChaosInvocationResp
	{
		public string DataTypeFullName { get; set; }
		public byte[] Data { get; set; }
		public SerializeException Exception { get; set; }

		public override string ToString()
		{
			if (Data == null || Data.Length == 0)
			{
				return "(null)";
			}

			object value = null;
			try
			{
				var converter = new ChaosConverter(new ChaosBinarySerializer());
				value = converter.ToData(this);
			}
			catch
			{
				var converter = new ChaosConverter(new ChaosJsonSerializer());
				value = converter.ToData(this);
			}

			return $"{value}";
		}
	}
}