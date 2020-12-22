using System.Collections.Generic;
using FlashElf.ChaosKit;

namespace ChaosAspNetSiteSample.Models.Services
{
	[ChaosInterface]
	public interface IMyRepo
	{
		Customer GetCustomer();
		Dictionary<int, string> GetIntStringDict();
	}
}