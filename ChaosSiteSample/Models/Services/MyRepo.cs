using System.Threading.Tasks;

namespace ChaosSiteSample.Models.Services
{
	public class MyRepo : IMyRepo
	{
		public Customer GetCustomer()
		{
			return new Customer()
			{
				Name = "flash"
			};
		}

		public Task<string> GetNameAsync()
		{
			return Task.FromResult("FlashAsync");
		}
	}
}