using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.Configuration.Settings
{
	public class AMQPSettings
	{
		public string AMQPUrl { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
