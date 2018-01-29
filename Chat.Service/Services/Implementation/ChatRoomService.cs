using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.Model;
using Chat.Repository.Interface;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Service.Services.Interface;
using Chat.Infrastructure.Enum;
using System;

namespace Chat.Service.Services.Implementation
{
	public class ChatRoomService : BaseService, IChatRoomService
	{
		[ThreadStatic]
		private IChatRoomRepository _chatRoomRepository;

		public ChatRoomService(
			IChatRoomRepository chatRoomRepository,
			IConfigurationManager configManager) : base(configManager)
		{
			_chatRoomRepository = chatRoomRepository;
		}

		public async Task<IList<ChatRoom>> GetChatRoomsForUserAsync(string username)
		{
			var rooms = await _chatRoomRepository.FindAllAsync(room => room.Users.Contains(username));
			return rooms.OrderByDescending(room => room.LastMessageTime).ToList();
		}

		public async Task CreateChatRoomAsync(ChatRoom room)
		{
			await _chatRoomRepository.InsertAsync(room);
		}

		public void CreateChatRoom(ChatRoom room)
		{
			if (room.Type == ChatRoomType.Single)
			{
				var rooms = _chatRoomRepository.FindAll(null);
				if (!rooms.Any(r => r.Users.Except(room.Users).ToList().Count <= 0))
					_chatRoomRepository.Insert(room);
			}
		}
	}
}