using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.Enum;
using Chat.Infrastructure.Helpers;
using Chat.Infrastructure.Model;
using Chat.Infrastructure.Model.OperationResult;
using Chat.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
	[Authorize]
	public class ChatRoomController : BaseController
	{
		private IChatRoomService _chatRoomService;
		private IChatMessageService _chatMessageService;
		private UserManager<ApplicationUser> _userManager;
		public IModel _channel { get; set; }


		public ChatRoomController(
			IConfigurationManager configManager,
			IChatRoomService chatRoomService,
			IChatMessageService chatMessageService,
			UserManager<ApplicationUser> userManager,
			IConnection connection

			)
			: base(configManager)
		{
			_chatRoomService = chatRoomService;
			_chatMessageService = chatMessageService;
			_userManager = userManager;
			_channel = connection.CreateModel();
		}

		[HttpPost]
		public async Task<IActionResult> GetChatRoomsForUser([FromBody] JObject jObject)
		{

			var username = jObject["username"].ToString();

			if (username != User.Identity.Name)
				throw new Exception("Identity Mismatch!");

			var rooms = await _chatRoomService.GetChatRoomsForUserAsync(username);
			foreach (var room in rooms)
				room.Messages = (await _chatMessageService.GetRoomMessages(room.Id));
			return new ObjectResult(rooms);
		}

		public async Task<IActionResult> CreateSingleChatRoom([FromBody] JObject jObject)
		{
			var result = new OperationResult<ChatRoom>();

			var username = jObject["username"].ToString();
			var greeting = jObject["message"] != null ? jObject["message"].ToString() : null;

			if (await _userManager.FindByNameAsync(username) == null)
				return new ObjectResult(await result.Fail(EnumHelper.GetStringValue(AuthError.UserNotFound)));

			var chatRoom = new ChatRoom
			{
				Id = ObjectId.GenerateNewId().ToString(),
				Users = new List<string> { User.Identity.Name, username },
				Messages = new List<ChatMessage>(),
				Type = ChatRoomType.Single,
				LastMessageTime = DateTime.Now,
				Name = username
			};

			if ((await _chatRoomService.GetChatRoomsForUserAsync(User.Identity.Name))
				.Any(r => r.Users.Contains(username)))
				return new ObjectResult(await result.Fail(EnumHelper.GetStringValue(ChatError.RoomAlreadyExists)));
			await _chatRoomService.CreateChatRoomAsync(chatRoom);

			if (greeting != null)
			{
				var greetingMessage = new ChatMessage
				{
					Id = ObjectId.GenerateNewId().ToString(),
					From = User.Identity.Name,
					To = username,
					Timestamp = DateTime.Now,
					RoomId = chatRoom.Id,
					Status = ChatMessageStatus.Sent,
					Body = greeting
				};

				var addHeaders = new Dictionary<string, object> { { "greeting", "true" } };
				await _chatMessageService.PublishMessageAsync(greetingMessage, addHeaders);
				chatRoom.Messages.Add(greetingMessage);
			}
			return new ObjectResult(await result.Success(chatRoom));
		}
	}
}