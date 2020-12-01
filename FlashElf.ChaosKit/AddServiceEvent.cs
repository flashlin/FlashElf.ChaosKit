using System;
using Microsoft.Extensions.DependencyInjection;

namespace FlashElf.ChaosKit
{
	public class AddServiceEvent : EventArgs
	{
		public ServiceDescriptor ServiceDescriptor { get; set; }
	}
}