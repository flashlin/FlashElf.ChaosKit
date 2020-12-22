using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ChaosAspNetSiteSample.Models.Services
{
	public class MyRepo : IMyRepo
	{
		public Customer GetCustomer()
		{
			return new Customer()
			{
				Name = "Flash"
			};
		}

		public Dictionary<int, string> GetIntStringDict()
		{
			return new Dictionary<int, string>()
			{
				{1, "abc"}
			};
		}
	}
}