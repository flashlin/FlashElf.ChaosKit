using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaosAspNetSiteSample.Models;
using ChaosAspNetSiteSample.Models.Services;

namespace ChaosAspNetSiteSample.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMyRepo _myRepo;

		public HomeController(IMyRepo myRepo)
		{
			_myRepo = myRepo;
		}

		public ActionResult Index()
		{
			var dict = _myRepo.GetIntStringDict();

			var vm = new HomeViewModel()
			{
				Customer = _myRepo.GetCustomer()
			};
			return View(vm);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}