using Chat.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Service.Services
{
    public class BaseService
    {
		protected IConfigurationManager _configManager;
		public BaseService(IConfigurationManager configManager)
		{
			_configManager = configManager;
		}
    }
}
