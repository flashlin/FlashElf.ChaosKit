using FlashElf.ChaosKit;

namespace ChaosSiteSample.Models.Services
{
	[ChaosInterface]
	public interface IMyRepo
	{
		Customer GetCustomer();
	}
}