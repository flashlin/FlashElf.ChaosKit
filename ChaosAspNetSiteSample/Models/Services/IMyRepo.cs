using FlashElf.ChaosKit;

namespace ChaosAspNetSiteSample.Models.Services
{
	[ChaosInterface]
	public interface IMyRepo
	{
		Customer GetCustomer();
	}
}