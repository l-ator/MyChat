using Chat.Infrastructure.Model;
using MongoDB.Driver;
using Chat.Infrastructure.Configuration.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Chat.Infrastructure.Enum;
using Chat.Repository.Interface;
using Chat.Repository.Mongo.Implementation;
using OIA.Common.Repository.Mongo.Interface;

namespace Chat.Repository.Implementation
{
	public class ChatMessageRepository : BaseMongoRepository<ChatMessage>, IChatMessageRepository
	{
		private readonly IMongoChatDbContext _dataContext;

		private readonly MongoDbSettings _mongoDbSettings;

		public ChatMessageRepository(
			IMongoChatDbContext dataContext,
			IOptions<MongoDbSettings> mongoDBSettings)
		{
			_mongoDbSettings = mongoDBSettings.Value;
			_dataContext = dataContext;
		}

		public override IMongoCollection<ChatMessage> Collection =>
			_dataContext.MongoDatabase.GetCollection<ChatMessage>(_mongoDbSettings.ChatSettings.MessagesCollectionName);

		public async Task<UpdateResult> SetMessageStatusAsync(string messageId, ChatMessageStatus status)
		{
			var update = Builders<ChatMessage>.Update.Set(m => m.Status, status);
			var result = await Collection.UpdateOneAsync(m=>m.Id ==messageId, update);
			return result;
		}
	}
}