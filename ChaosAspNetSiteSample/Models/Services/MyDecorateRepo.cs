using System.Collections.Generic;

namespace ChaosAspNetSiteSample.Models.Services
{
	public class MyDecorateRepo : IMyRepo
	{
		private IMyRepo _next;

		public MyDecorateRepo(IMyRepo next)
		{
			_next = next;
		}

		public Customer GetCustomer()
		{
			return _next.GetCustomer();
		}

		public Dictionary<int, string> GetIntStringDict()
		{
			return _next.GetIntStringDict();
		}
	}
}