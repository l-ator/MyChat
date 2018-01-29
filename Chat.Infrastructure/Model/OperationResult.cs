using Chat.Infrastructure.Enum;
using Chat.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.Model.OperationResult
{
	public class OperationResult
	{
		public bool Succeeded { get; set; }
		public List<string> Errors { get; set; } = new List<string>();

		public async Task<OperationResult> Success()
		{
			Succeeded = true;
			return await Task.FromResult(this);
		}

		public async Task<OperationResult> Fail()
		{
			Succeeded = false;
			if (Errors.Count <= 0)
				Errors.Add(EnumHelper.GetStringValue(Error.GenericError));
			return await Task.FromResult(this);
		}

		public async Task<OperationResult> Fail(string error)
		{
			Errors.Add(error);
			return await Fail();
		}

		public async Task<OperationResult> Fail(IEnumerable<string> errors)
		{
			Errors.AddRange(errors);
			return await Fail();
		}

		public void AddError(string error) => Errors.Add(error);
	}

	public class OperationResult<T> : OperationResult where T : class
	{
		public T Content { get; set; }

		public async Task<OperationResult> Success(T content)
		{
			Succeeded = true;
			Content = content;
			return await Task.FromResult(this);
		}
	}

	public class AuthOperationResult : OperationResult
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
	}
}
