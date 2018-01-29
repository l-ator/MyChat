using Chat.Infrastructure.Model;
using Chat.Repository.Mongo.Interface;

namespace Chat.Repository.Interface
{
	public interface IUserRepository : IMongoRepository<ApplicationUser>
	{

	}
}
