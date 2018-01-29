using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Chat.Infrastructure.Configuration.Settings;

namespace Chat.Infrastructure.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
		public AMQPSettings AMQPSettings { get; private set; }
		public RabbitMQSettings RabbitMQSettings { get; private set; }
		public StompSettings StompSettings { get; private set; }
		public MongoDbSettings MongoDbSettings { get; private set; }

		public ConfigurationManager(
			IOptions<AMQPSettings> amqpSettings,
			IOptions<RabbitMQSettings> rabbitMQSettings,
			IOptions<StompSettings> stompSettings,
			IOptions<MongoDbSettings> mongoDbSettings
			)
		{
			AMQPSettings = amqpSettings.Value;
			MongoDbSettings = mongoDbSettings.Value;
			StompSettings = stompSettings.Value;
			RabbitMQSettings = rabbitMQSettings.Value;
		}
    }
}
