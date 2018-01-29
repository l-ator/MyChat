using Chat.Infrastructure.Configuration.Settings;

namespace Chat.Infrastructure.Configuration
{
	public interface IConfigurationManager
	{
		AMQPSettings AMQPSettings { get; }
		RabbitMQSettings RabbitMQSettings { get; }
		MongoDbSettings MongoDbSettings { get; }
		StompSettings StompSettings { get; }
	}
}