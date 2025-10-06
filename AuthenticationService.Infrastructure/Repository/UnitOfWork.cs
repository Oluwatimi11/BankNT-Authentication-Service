using System;
using AuthenticationService.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthenticationService.Infrastructure.Repository
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private bool _disposedValue;
        private readonly AuthenticationServiceDbContext _context;
        private IDbContextTransaction _objTransaction;
        private IAppUserRepository _userRepository;

        public UnitOfWork(AuthenticationServiceDbContext context)
        {
            _context = context;
        }

        public IAppUserRepository UserRepository => _userRepository ??= new AppUserRepository(_context);

        public async Task CreateTransaction()
        {
            _objTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _objTransaction.CommitAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposedValue)
            {
                if(disposing)
                {
                    _context.Dispose();
                }

                _disposedValue = true;
            }
        }


        public async void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task Rollback()
        {
            await _objTransaction?.RollbackAsync();
            await _objTransaction.DisposeAsync();
        }

        public async Task Save()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}

