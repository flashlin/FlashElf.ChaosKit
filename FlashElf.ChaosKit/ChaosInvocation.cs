using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class ChaosInvocation
	{
		public string InterfaceTypeFullName { get; set; }
		public ChaosParameter[] GenericTypes { get; set; }
		public string MethodName { get; set; }
		public List<ChaosParameter> Parameters { get; set; } = new List<ChaosParameter>();
		public string ReturnTypeFullName { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("{");
			sb.Append($"{nameof(InterfaceTypeFullName)}: '{InterfaceTypeFullName}', ");
			sb.Append($"{nameof(GenericTypes)}: {GenericTypes.GetString()}, ");
			sb.Append($"{nameof(Parameters)}: {Parameters.GetString()}, ");
			sb.Append($"{nameof(ReturnTypeFullName)}: '{ReturnTypeFullName}'");
			sb.Append("}");
			return sb.ToString();
		}
	}

	public static class DisplayExtension
	{
		public static string GetString<T>(this IEnumerable<T> enumer)
		{
			if (enumer == null)
			{
				return string.Empty;
			}
			var sb = new StringBuilder();
			foreach (var item in enumer)
			{
				if (sb.Length > 0)
				{
					sb.Append(", ");
				}
				sb.Append($"{item}");
			}
			return "[" + sb.ToString() + "]";
		}
	}
}