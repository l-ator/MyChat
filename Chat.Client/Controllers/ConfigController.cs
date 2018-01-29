using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chat.Infrastructure.Configuration;

namespace Chat.Client.Controllers
{
	public class ConfigController: BaseController 
	{
		public ConfigController(
				IConfigurationManager configManager
			) : base(configManager) { }

		public async Task<IActionResult> GetRabbitMQSettings() => await Task.FromResult(new ObjectResult(_configManager.RabbitMQSettings));
		public async Task<IActionResult> GetAMQPSettings() => await Task.FromResult(new ObjectResult(_configManager.AMQPSettings));
		public async Task<IActionResult> GetStompSettings() => await Task.FromResult(new ObjectResult(_configManager.StompSettings));
		public async Task<IActionResult> GetMongoDBSettings() => await Task.FromResult(new ObjectResult(_configManager.MongoDbSettings));
	}
}