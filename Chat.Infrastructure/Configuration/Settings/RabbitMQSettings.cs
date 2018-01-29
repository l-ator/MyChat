using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.Configuration.Settings
{
	public class RabbitMQSettings
	{
		public string HostUrl { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
