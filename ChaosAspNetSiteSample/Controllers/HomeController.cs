using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaosAspNetSiteSample.Models.Services;

namespace ChaosAspNetSiteSample.Controllers
{
	public class HomeController : Controller
	{
		private IMyRepo _myRepo;

		public HomeController(IMyRepo myRepo)
		{
			_myRepo = myRepo;
		}

		public ActionResult Index()
		{
			var customer = _myRepo.GetCustomr();
			return View();
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