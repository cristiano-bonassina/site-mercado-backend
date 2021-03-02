using System;
using LogicArt.Arch.Application.Repositories.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Application.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
