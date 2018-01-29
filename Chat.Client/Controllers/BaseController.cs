using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chat.Infrastructure.Configuration;
using RabbitMQ.Client;

namespace Chat.Client.Controllers
{
	public class BaseController : Controller
	{
		public IConfigurationManager _configManager { get; set; }

		public BaseController(
				IConfigurationManager configManager
			)
		{
			_configManager = configManager;
		}
	}
}