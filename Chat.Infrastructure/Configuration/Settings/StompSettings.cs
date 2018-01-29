using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.Configuration.Settings
{
	public class StompSettings
	{
		public string HostUrl { get; set; }
		public int HeartbeatIn { get; set; }
		public int HeartbeatOut { get; set; }
		public int ReconnectDelay { get; set; }
		public bool Debug { get; set; }
	}
}
