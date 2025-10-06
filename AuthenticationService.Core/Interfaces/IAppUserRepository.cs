using System;
using AuthenticationService.Domain.Models;

namespace AuthenticationService.Core.Interfaces
{
	public interface IAppUserRepository : IGenericRepository<AppUser>
	{
	}
}

