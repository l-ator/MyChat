using AspNetCore.Identity.MongoDB;

namespace Chat.Infrastructure.Model
{
	public class ApplicationUser : MongoIdentityUser, IEntity
	{
	}
}
