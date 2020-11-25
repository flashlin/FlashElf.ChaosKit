using System;
using System.Collections.Generic;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class ChaosRepoInvocation
	{
		public List<ChaosParameter> Parameters { get; set; } = new List<ChaosParameter>();
		public string InterfaceName { get; set; }
		public string MethodName { get; set; }
	}
}