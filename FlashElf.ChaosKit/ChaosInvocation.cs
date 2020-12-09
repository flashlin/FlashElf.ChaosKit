using System;
using System.Collections.Generic;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class ChaosInvocation
	{
		public List<ChaosParameter> Parameters { get; set; } = new List<ChaosParameter>();
		public string InterfaceName { get; set; }
		public string MethodName { get; set; }
		public ChaosParameter[] GenericTypes { get; set; }
		public string ReturnTypeFullName { get; set; }
	}
}