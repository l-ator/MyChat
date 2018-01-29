using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chat.Infrastructure.Configuration;

namespace Chat.Client.Controllers
{
	public class HomeController : BaseController
	{

		public HomeController(IConfigurationManager configManager): base(configManager)
		{
			
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Error()
		{
			ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
			return View();
		}
	}
}
