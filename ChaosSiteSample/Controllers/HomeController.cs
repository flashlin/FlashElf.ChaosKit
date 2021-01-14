using System.Diagnostics;
using ChaosSiteSample.Models;
using ChaosSiteSample.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChaosSiteSample.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IMyRepo _myRepo;
		private readonly IDecorateRepo _decorateRepo;

		public HomeController(ILogger<HomeController> logger, 
			IMyRepo myRepo,
			IDecorateRepo decorateRepo)
		{
			_decorateRepo = decorateRepo;
			_myRepo = myRepo;
			_logger = logger;
		}

		public IActionResult Index()
		{
			var vm = new HomeViewModel()
			{
				Customer = _myRepo.GetCustomer(),
				Name = _decorateRepo.GetName()
			};
			return View(vm);
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
