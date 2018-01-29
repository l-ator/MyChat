using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.Enum;
using Chat.Infrastructure.Model;
using Chat.Repository.Interface;
using Chat.Service.Consumers.Interface;
using Chat.Service.Services.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Consumers.Implementation
{
	public class StatusUpdatePersistor : BaseConsumer, IStatusUpdatePersistor
	{
		//private IChatMessageService _chatMessageService;
		private IChatMessageRepository _chatMessageRepository;

		public StatusUpdatePersistor(
			IConfigurationManager configManager,
			IConnection connection,
			IChatMessageRepository chatMessageRepository
			) : base(configManager, connection)
		{
			_chatMessageRepository = chatMessageRepository;

			var exchangeName = RabbitConst.StatuUpdateExchange;

			var persistorQueueName = _channel.QueueDeclare(queue: RabbitConst.StatusPersistorQueue,
														   durable: true,
														   exclusive: false,
														   autoDelete: false,
														   arguments: null)
														   .QueueName;

			_channel.ExchangeDeclare(exchange: exchangeName,
									 type: ExchangeType.Topic,
									 durable: true,
									 autoDelete: false,
									 arguments: null);

			_channel.QueueBind(exchange: exchangeName,
							   queue: persistorQueueName,
							   routingKey: "update.*.to.*",
							   arguments: null);

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (sndr, ea) =>
			{
				var headers = ea.BasicProperties.Headers;

				Func<object, string> Decode = value => Encoding.UTF8.GetString((byte[])value);

				var msgId = Decode(headers[RabbitConst.IdHeader]);
				var msgStatus = (ChatMessageStatus)int.Parse(Decode(headers[RabbitConst.StatusHeader]));

				Task.Run(() => _chatMessageRepository.SetMessageStatusAsync(msgId,msgStatus).Wait());
				_channel.BasicAck(ea.DeliveryTag, false);
			};

			_channel.BasicConsume(queue: persistorQueueName,
								  autoAck: false,
								  consumer: consumer);
		}
	}
}
