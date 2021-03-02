using System;
using LogicArt.Arch.Infrastructure.Persistence.EntityFramework;
using LogicArt.SiteMercado.Application.Repositories.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(ArchDbContext context) : base(context)
        {
        }
    }
}
