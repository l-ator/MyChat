using MongoDB.Driver;

namespace OIA.Common.Repository.Mongo.Interface
{
	public interface IMongoChatDbContext
	{
		IMongoDatabase MongoDatabase { get; }
	}
}