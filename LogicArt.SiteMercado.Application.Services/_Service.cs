using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogicArt.Arch.Application.Repositories.Abstractions;
using LogicArt.Arch.Domain.Entities.Abstractions;
using LogicArt.SiteMercado.Core.Services.Abstractions;

namespace LogicArt.SiteMercado.Application.Services
{
    public class Service<TEntity, TKey> : IService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Service(IRepository<TEntity, TKey> repository) => this.Repository = repository;

        protected IRepository<TEntity, TKey> Repository { get; }

        public Task<TEntity> AddAsync(TEntity entity) => this.Repository.AddAsync(entity);

        public virtual IAsyncEnumerable<TEntity> GetAllAsync()
        {
            var entities = this.Repository.Query().ToList();
            return entities.OrderBy(x => x.ToString()).ToAsyncEnumerable();
        }

        public TEntity FindById(TKey id) => this.Repository.FindById(id);

        public ValueTask<TEntity> FindByIdAsync(TKey id) => this.Repository.FindByIdAsync(id);

        public void Remove(TEntity entity) => this.Repository.Remove(entity);
    }
}
