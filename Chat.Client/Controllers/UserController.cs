using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chat.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Chat.Infrastructure.Model.OperationResult;
using Chat.Infrastructure.Helpers;
using Chat.Infrastructure.Enum;
using Newtonsoft.Json.Linq;
using Chat.Infrastructure.Model;
using Chat.Service.Services.Interface;
using System.Collections;
using System.Collections.Generic;

namespace Chat.Client.Controllers
{
	public class UserController : BaseController
	{
		private IUserService _userService;
		private UserManager<ApplicationUser> _userManager;

		public UserController(
			IConfigurationManager configManager,
			IUserService userService,
		UserManager<ApplicationUser> userManager
			)
			: base(configManager)
		{
			_userService = userService;
			_userManager = userManager;
		}

		//public async Task<IActionResult> GetAllUsers()
		//{
		//	_userManager.GetAll
		//}

		public async Task<IActionResult> GetAllUsers() {

			var result = new OperationResult<IEnumerable<string>>();
			var users = await _userService.GetAllUsernamesAsync();
			return new ObjectResult(await result.Success(users));
		}

		[HttpPost]
		public async Task<IActionResult> UserExists([FromBody] JObject jObject)
		{
			var result = new OperationResult<ApplicationUser>();
			var username = jObject["username"].ToString();
			var user = await _userManager.FindByNameAsync(username);
			if (user != null)
				return new ObjectResult(await result.Success(user));
			else
				return new ObjectResult(await result.Fail(EnumHelper.GetStringValue(AuthError.UserNotFound)));
		}
	}
}