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

namespace Chat.Service.Services.Interface
{
	public interface IChatMessageService
	{
		Task<List<ChatMessage>> GetRoomMessages(string roomId, uint fetchCount = 0);
		Task PublishMessageAsync(ChatMessage message, Dictionary<string, object> additionalHeaders);
		Task PublishStatusUpdateAsync(List<string> ids, ChatMessageStatus status);
	}
}