using System.Diagnostics;
using System.Threading.Tasks;
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
		private ISingletonRepo _singletonRepo;

		public HomeController(ILogger<HomeController> logger, 
			IMyRepo myRepo,
			IDecorateRepo decorateRepo,
			ISingletonRepo singletonRepo)
		{
			_singletonRepo = singletonRepo;
			_decorateRepo = decorateRepo;
			_myRepo = myRepo;
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			var vm = new HomeViewModel()
			{
				Customer = _myRepo.GetCustomer(),
				Name = _decorateRepo.GetName(),
				SingletonName = _singletonRepo.GetSingletonName(),
				NameAsync = await _myRepo.GetNameAsync()
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
