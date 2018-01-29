using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.Enum;
using Chat.Infrastructure.Model;
using Chat.Infrastructure.Model.OperationResult;
using Chat.Repository.Interface;
using Chat.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Chat.Client.Controllers
{
	public class ChatMessageController : BaseController
	{
		const string MOCK_ROOM = "mock";
		private IChatMessageService _chatMessageService;
		public IModel _channel { get; set; }

		public ChatMessageController(
			IConfigurationManager configManager,
			IChatMessageService chatMessageService,
			IChatRoomRepository repo,
			IConnection connection
			)
			: base(configManager)
		{
			_chatMessageService = chatMessageService;
			_channel = connection.CreateModel();
		}

		[HttpPost]
		public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
		{
			var username = User.Identity.Name;

			var message = new ChatMessage()
			{
				Id = model.Id ?? ObjectId.GenerateNewId().ToString(),
				RoomId = model.Room.Id ?? ObjectId.GenerateNewId().ToString(),
				From = User.Identity.Name,
				Timestamp = DateTime.Now,
				Status = ChatMessageStatus.Sending,
				Body = model.Body
			};

			switch (model.Room.Type)
			{
				case ChatRoomType.Single:
					message.To = model.Room.Users.FirstOrDefault(u => u != username);
					break;
				case ChatRoomType.Group:
					message.To = $"group_{model.Room.Id}";
					break;
				default:
					throw new InvalidOperationException();
			}

			var headers = new Dictionary<string, object>();
			if (model.Session != null)
				headers["session"] = model.Session;

				await _chatMessageService.PublishMessageAsync(message, headers);
			return new ObjectResult(message);
		}


		[HttpPost]
		public async Task<IActionResult> UpdateMessageStatus([FromBody] UpdateMessagesModel model)
		{
			var result = new OperationResult();
			try
			{
				await _chatMessageService.PublishStatusUpdateAsync(model.Ids, model.Status);
				return new ObjectResult(result.Success());
			}
			catch
			{
				return new ObjectResult(result.Fail());
			}
		}
	}

	public class SendMessageModel { public string Id { get; set; } public ChatRoom Room { get; set; } public string Body { get; set; } public string Session { get; set; } }
	public class UpdateMessagesModel { public List<string> Ids { get; set; } public ChatMessageStatus Status { get; set; } }
}