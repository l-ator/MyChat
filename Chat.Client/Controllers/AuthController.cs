using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chat.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Chat.Infrastructure.Model.OperationResult;
using Chat.Infrastructure.Helpers;
using Chat.Infrastructure.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using RabbitMQ.Client;
using MongoDB.Bson;
using Chat.Infrastructure.Attributes;
using Chat.Infrastructure.Model;

namespace Chat.Client.Controllers
{
	public class AuthController : BaseController
	{
		private UserManager<ApplicationUser> _userManager;
		private SignInManager<ApplicationUser> _signInManager;
		private IModel _channel;

		public AuthController(
			IConfigurationManager configManager,
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IConnection connection
			)
			: base(configManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_channel = connection.CreateModel();
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
		{
			var username = loginModel.Username.ToLower();
			var password = loginModel.Password;
			bool rememberMe = loginModel.RememberMe ?? false;

			var result = new AuthOperationResult() { Username = username };
			try
			{
				if (User.Identity.IsAuthenticated)
				{
					return new ObjectResult(await result.Fail(EnumHelper.GetStringValue(AuthError.AlreadyAuthenticated)));

				}

				var user = await _userManager.FindByNameAsync(username);
				if (user == null)
				{
					result.AddError(EnumHelper.GetStringValue(AuthError.UserNotFound));
					return new ObjectResult(await result.Fail());
				}

				var signInResult = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: true);
				if (!signInResult.Succeeded)
				{
					if (signInResult.IsNotAllowed)
						return new ObjectResult(await result.Fail(EnumHelper.GetStringValue(AuthError.InvalidCredentials)));
					else if (signInResult.IsLockedOut)
						return new ObjectResult(await result.Fail(EnumHelper.GetStringValue(AuthError.LockedOut)));
					else
						return new ObjectResult(await result.Fail(EnumHelper.GetStringValue(AuthError.InvalidCredentials)));
				}
				result.Id = user.Id;
				result.Email = user.Email;
				var sessionId = ObjectId.GenerateNewId().ToString();

				await ValidateSessionAttribute.SetSessionid(username, sessionId);
				HttpContext.Session.SetString("sessionId", sessionId);
				return new ObjectResult(await result.Success());
			}
			catch
			{
				return new ObjectResult(await result.Fail());
			}
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			var result = new AuthOperationResult();
			try
			{
				await _signInManager.SignOutAsync();
				return new ObjectResult(await result.Success());
			}
			catch
			{
				return new ObjectResult(await result.Fail());
			}
		}

		[HttpPost]
		public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
		{
			var username = registerModel.Username.ToLower();
			var password = registerModel.Password;
			var email = registerModel.Email;

			var result = new AuthOperationResult() { Username = username };
			try
			{
				var user = await _userManager.FindByNameAsync(username);
				if (user != null)
					return new ObjectResult(
						await result.Fail(EnumHelper.GetStringValue(AuthError.UserAlreadyExists)));

				user = new ApplicationUser() { UserName = username, Email = email };
				var createResult = await _userManager.CreateAsync(user, password);
				if (!createResult.Succeeded)
					return new ObjectResult(await result.Fail(
							createResult.Errors.Select(e => e.Description).ToList()));

				await _signInManager.SignInAsync(user, isPersistent: false);
				result.Id = user.Id;
				result.Username = user.UserName;
				result.Email = user.Email;

				var sessionId = ObjectId.GenerateNewId().ToString();
				await ValidateSessionAttribute.SetSessionid(username, sessionId);
				HttpContext.Session.SetString("sessionId", sessionId);

				return new ObjectResult(await result.Success());

			}
			catch
			{
				return new ObjectResult(await result.Fail());
			}
		}

		public async Task<IActionResult> CheckAuth()
		{
			var result = new AuthOperationResult() { Username = null };
			try
			{
				if (User.Identity.IsAuthenticated)
				{
					var user = await _userManager.FindByNameAsync(User.Identity.Name);
					result.Username = User.Identity.Name;
				}
				return new ObjectResult(await result.Success());
			}
			catch
			{
				return new ObjectResult(await result.Fail());
			}
		}

		public class LoginModel
		{
			public string Username;
			public string Password;
			public bool? RememberMe;
		}

		public class RegisterModel
		{
			public string Username;
			public string Email;
			public string Password;
		}
	}
}