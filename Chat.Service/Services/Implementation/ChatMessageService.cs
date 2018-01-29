using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.Model;
using Chat.Repository.Interface;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chat.Infrastructure.Enum;
using MongoDB.Driver;
using MongoDB.Bson;
using Chat.Service.Services.Interface;
using RabbitMQ.Client;

namespace Chat.Service.Services.Implementation
{
	public class ChatMessageService : BaseService, IChatMessageService
	{
		private const string MESSAGE_EXCHANGE = "message_exchange";
		private const string STATUS_UPDATE_EXCHANGE = "status_update_exchange";

		private IConnection _connection;
		private IModel _channel;
		private IChatMessageRepository _chatMessageRepository;

		public ChatMessageService(
			IConfigurationManager configManager,
			IChatMessageRepository chatMessageRepository,
			IConnection connection
			)
			: base(configManager)
		{
			_connection = connection;
			_channel = _connection.CreateModel();
			_chatMessageRepository = chatMessageRepository;
		}

		public async Task<List<ChatMessage>> GetRoomMessages(string roomId, uint fetchCount = 0)
		{
			var messages = (await _chatMessageRepository.FindAllAsync(m => m.RoomId.Equals(roomId)))
				.OrderBy(m => m.Timestamp)
				.ToList();

			if (fetchCount > 0)
				return messages.Take((int)fetchCount).ToList();
			return messages.ToList();
		}

		public async Task PublishMessageAsync(ChatMessage message, Dictionary<string, object> additionalHeaders = null)
		{
			_channel.ExchangeDeclare(exchange: MESSAGE_EXCHANGE,
								 type: ExchangeType.Topic,
								 durable: true,
								 autoDelete: false);

			var props = _channel.CreateBasicProperties();
			props.Headers = new Dictionary<string, object>
			{
				{RabbitConst.IdHeader,message.Id },
				{RabbitConst.RoomIdHeader, message.RoomId },
				{RabbitConst.FromHeader, message.From },
				{RabbitConst.ToHeader, message.To },
				{RabbitConst.TimestampHeader, new DateTimeOffset(message.Timestamp).ToUnixTimeSeconds().ToString()},
				{RabbitConst.StatusHeader, message.Status.ToString()}
			};

			if (additionalHeaders != null)
				foreach(var header in additionalHeaders)
					props.Headers.Add(header);

			_channel.BasicPublish(
				exchange: MESSAGE_EXCHANGE,
				routingKey: $"msg.{message.From}.to.{message.To}",
				mandatory: true,
				basicProperties: props,
				body: Encoding.UTF8.GetBytes(message.Body));
		}

		public async Task PublishStatusUpdateAsync(List<string> ids, ChatMessageStatus status)
		{
			_channel.ExchangeDeclare(exchange: STATUS_UPDATE_EXCHANGE,
								 type: ExchangeType.Topic,
								 durable: true,
								 autoDelete: false);

			var messages = (await _chatMessageRepository.FindAllAsync(msg => ids.Contains(msg.Id))).OrderBy(msg => msg.Timestamp);

			foreach (var message in messages)
			{

				var props = _channel.CreateBasicProperties();
				props.Headers = new Dictionary<string, object>
					{
						{RabbitConst.IdHeader,message.Id },
						{RabbitConst.RoomIdHeader, message.RoomId },
						//{"timestamp", new DateTimeOffset(message.Timestamp).ToUnixTimeSeconds().ToString()},
						{RabbitConst.StatusHeader, ((int)status).ToString() }
					};

				_channel.BasicPublish(
					exchange: STATUS_UPDATE_EXCHANGE,
					routingKey: $"update.{message.From}.to.{message.To}",
					mandatory: true,
					basicProperties: props,
					body: new byte[0]);
			}
		}
	}
}