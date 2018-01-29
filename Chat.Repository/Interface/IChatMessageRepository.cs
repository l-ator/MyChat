using Chat.Infrastructure.Enum;
using Chat.Infrastructure.Model;
using Chat.Repository.Mongo;
using Chat.Repository.Mongo.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Repository.Interface
{
	public interface IChatMessageRepository : IMongoRepository<ChatMessage>
	{
		Task<UpdateResult> SetMessageStatusAsync(string messageId, ChatMessageStatus status);
		//Task<UpdateResult> AddMessageToRoomAsync(string roomId, ChatMessage message);
	}
}
