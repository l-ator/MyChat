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
	public class UserRepository : BaseMongoRepository<ApplicationUser>, IUserRepository
	{
		private readonly IMongoUserDbContext _dataContext;

		private readonly MongoDbSettings _mongoDbSettings;

		public UserRepository(
			IMongoUserDbContext dataContext,
			IOptions<MongoDbSettings> mongoDBSettings)
		{
			_mongoDbSettings = mongoDBSettings.Value;
			_dataContext = dataContext;
		}

		public override IMongoCollection<ApplicationUser> Collection =>
			_dataContext.MongoDatabase.GetCollection<ApplicationUser>(_mongoDbSettings.IdentitySettings.UserCollectionName);
	}
}