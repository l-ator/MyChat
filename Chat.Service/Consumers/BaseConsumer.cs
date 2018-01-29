using Chat.Infrastructure.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Service.Consumers.Implementation
{
    public class BaseConsumer
	{
		protected IConfigurationManager _configManager;
		protected IConnection _connection;
		protected IModel _channel;

		public BaseConsumer(
			IConfigurationManager configManager,
			IConnection connection)
		{
			_configManager = configManager;
			_connection = connection;
			_channel = _connection.CreateModel();
		}

    }
}
