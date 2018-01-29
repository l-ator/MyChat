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
	public class MessagePersistor : BaseConsumer, IMessagePersistor
	{
		//private IChatMessageService _chatMessageService;
		private IChatMessageRepository _chatMessageRepository;

		public MessagePersistor(
			IConfigurationManager configManager,
			IConnection connection,
			IChatMessageRepository chatMessageRepository
			) : base(configManager, connection)
		{
			_chatMessageRepository = chatMessageRepository;

			var exchangeName = RabbitConst.MessageExchange;

			var persistorQueueName = _channel.QueueDeclare(queue: RabbitConst.MessagePersistorQueue,
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
							   routingKey: "msg.*.to.*",
							   arguments: null);

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (sndr, ea) =>
			{
				var headers = ea.BasicProperties.Headers;


				var message = new ChatMessage
				{
					Id = Decode(headers[RabbitConst.IdHeader]),
					RoomId = Decode(headers[RabbitConst.RoomIdHeader]),
					From = Decode(headers[RabbitConst.FromHeader]),
					To = Decode(headers[RabbitConst.ToHeader]),
					Timestamp = ToDateTime(Decode(headers[RabbitConst.TimestampHeader])),
					Body = Decode(ea.Body),
					Status = ChatMessageStatus.Sent
				};

				Task.Run(() => _chatMessageRepository.InsertAsync(message)).Wait();
				_channel.BasicAck(ea.DeliveryTag, false);
			};

			_channel.BasicConsume(queue: persistorQueueName,
								  autoAck: false,
								  consumer: consumer);
		}

		private Func<object, string> Decode = value => Encoding.UTF8.GetString((byte[])(value!=null?value:new byte[0]));
		private Func<string, DateTime> ToDateTime = value =>
		{
			if (long.TryParse(value, out var timestamp))
				return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
			else throw new InvalidOperationException($"Supplied parameter [{value}] is not a valid timestamp");
		};
	}

}
