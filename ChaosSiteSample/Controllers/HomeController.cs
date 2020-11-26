using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChaosSite.Models;
using ChaosSite.Models.Services;

namespace ChaosSite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IMyRepo _myRepo;

		public HomeController(ILogger<HomeController> logger, IMyRepo myRepo)
		{
			_myRepo = myRepo;
			_logger = logger;
		}

		public IActionResult Index()
		{
			var data = _myRepo.GetCustomer();
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
