using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Microsoft.Extensions.Logging;

namespace ChaosAspNetSiteSample.Models.Services
{
	public class MyRepo : IMyRepo
	{
		private ILogger _logger;

		public MyRepo(ILogger logger)
		{
			_logger = logger;
		}

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