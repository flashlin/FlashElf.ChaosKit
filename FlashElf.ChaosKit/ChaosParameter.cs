using System;
using System.Text;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class ChaosParameter
	{
		public string ParameterType { get; set; }
		public string Name { get; set; }
		public byte[] Value { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("{");
			sb.Append($"{nameof(ParameterType)}: '{ParameterType}', ");
			sb.Append($"{nameof(Name)}: '{Name}', ");
			sb.Append($"{nameof(Value)}: {Value.GetString()}");
			sb.Append("}");
			return sb.ToString();
		}
	}
}