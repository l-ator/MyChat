using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Chat.Infrastructure.Model.OperationResult;

namespace Chat.Infrastructure.Attributes
{
	public class ValidateSessionAttribute : ActionFilterAttribute, IAsyncActionFilter
	{
		public static Dictionary<string, string> SessionDict { get; private set; } = new Dictionary<string, string>();
		public const string SESSION_ID = "sessionId";

		private readonly bool _allowAnonymous;

		public ValidateSessionAttribute(bool allowAnonymous = false)
		{
			_allowAnonymous = allowAnonymous;
		}

		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{

			if (context.HttpContext.Session.GetString(SESSION_ID) != SessionDict[context.HttpContext.User.Identity.Name])
			{
				context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
				return;
			}
			await next();
		}

		public async static Task SetSessionid(string username, string sessionId) => await Task.Run(() => SessionDict[username] = sessionId);
	}
}
