using Chat.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Services.Interface
{
	public interface IUserService
    {
		Task<IEnumerable<string>> GetAllUsernamesAsync();
	}
}
