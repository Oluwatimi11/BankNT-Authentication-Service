using System;
namespace AuthenticationService.Core.Interfaces
{
	public interface IUnitOfWork
	{
		IAppUserRepository UserRepository { get; }

        Task CreateTransaction();

        Task Commit();

		void Dispose();

		Task Rollback();

		Task Save();
	}
}

