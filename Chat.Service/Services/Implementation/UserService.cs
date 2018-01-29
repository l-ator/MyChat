using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.Model;
using Chat.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Service.Services.Interface
{
	public class UserService : BaseService, IUserService
	{
		private IUserRepository _userRepository;

		public UserService(
			IConfigurationManager configManager,
			IUserRepository userRespository)
			: base(configManager)
		{
			_userRepository = userRespository;
		}

		public async Task<IEnumerable<string>> GetAllUsernamesAsync()
		{
			var users = await _userRepository.FindAllAsync(u => true);
			return users.Select(u => u.UserName).ToList();
		}

	}
}
