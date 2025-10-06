using System;
using AuthenticationService.Core.Interfaces;
using AuthenticationService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Repository
{
	public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
	{
		private readonly AuthenticationServiceDbContext _context;
		private readonly DbSet<AppUser> _db;

		public AppUserRepository(AuthenticationServiceDbContext context) : base(context)
		{
			_context = context;
			_db = _context.Set<AppUser>();
		}
	}
}

