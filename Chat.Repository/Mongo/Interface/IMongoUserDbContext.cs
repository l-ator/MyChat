using MongoDB.Driver;

namespace OIA.Common.Repository.Mongo.Interface
{
	public interface IMongoUserDbContext
	{
		IMongoDatabase MongoDatabase { get; }
	}
}