using Chat.Infrastructure.Model;
using MongoDB.Driver;
using Chat.Infrastructure.Configuration.Settings;
using Microsoft.Extensions.Options;
using Chat.Repository.Interface;
using Chat.Repository.Mongo.Implementation;
using OIA.Common.Repository.Mongo.Interface;

namespace Chat.Repository.Implementation
{
	public class ChatRoomRepository : BaseMongoRepository<ChatRoom>, IChatRoomRepository
	{
		private readonly IMongoChatDbContext _dataContext;

		private readonly MongoDbSettings _mongoDbSettings;

		public ChatRoomRepository(
			IMongoChatDbContext dataContext,
			IOptions<MongoDbSettings> mongoDBSettings)
		{
			_mongoDbSettings = mongoDBSettings.Value;
			_dataContext = dataContext;
		}

		public override IMongoCollection<ChatRoom> Collection =>
			_dataContext.MongoDatabase.GetCollection<ChatRoom>(_mongoDbSettings.ChatSettings.RoomsCollectionName);

	}
}