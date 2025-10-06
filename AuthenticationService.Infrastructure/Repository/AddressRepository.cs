using System;
using AuthenticationService.Core.Interfaces;
using AuthenticationService.Domain.Models;

namespace AuthenticationService.Infrastructure.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AuthenticationServiceDbContext context) : base(context)
        {
            
        }
    }
}



