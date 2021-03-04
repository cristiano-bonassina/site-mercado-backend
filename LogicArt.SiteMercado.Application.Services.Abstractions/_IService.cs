using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicArt.Arch.Domain.Entities.Abstractions;

namespace LogicArt.SiteMercado.Core.Services.Abstractions
{
    public interface IService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync();

        TEntity FindById(TKey id);

        ValueTask<TEntity> FindByIdAsync(TKey id);

        void Remove(TEntity entity);
    }
}
