using Chat.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Services.Interface
{
	public interface IChatRoomService
    {
		Task<IList<ChatRoom>> GetChatRoomsForUserAsync(string username);
		Task CreateChatRoomAsync(ChatRoom room);
		void CreateChatRoom(ChatRoom room);
	}
}
