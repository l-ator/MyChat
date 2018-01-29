using Chat.Infrastructure.Model;
using Chat.Repository.Mongo.Interface;

namespace Chat.Repository.Interface
{
	public interface IChatRoomRepository : IMongoRepository<ChatRoom>
	{
	}
}
