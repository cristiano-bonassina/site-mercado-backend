using System;
using System.Collections.Generic;
using System.Linq;
using LogicArt.Arch.Application.Data;
using LogicArt.Arch.Domain.Entities;
using LogicArt.SiteMercado.Application.Adapters.Abstractions;
using LogicArt.SiteMercado.Core.Services.Abstractions;

namespace LogicArt.SiteMercado.Application.Adapters
{
    public abstract class Adapter<TEntity, TResource, TKey> : IAdapter<TEntity, TResource>
        where TEntity : Entity<TKey>
        where TResource : Resource<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IService<TEntity, TKey> _service;

        protected Adapter(IService<TEntity, TKey> service) => _service = service;

        public TEntity? Adapt(TResource? resource)
        {
            if (resource == null)
            {
                return null;
            }

            var entity = this.RecoverInstanceAsync(resource);
            if (entity == null)
            {
                return default;
            }

            this.WriteEntity(resource, entity);

            return entity;
        }

        public TResource? Adapt(TEntity? entity)
        {
            if (entity == null)
            {
                return null;
            }

            var resource = this.CreateResourceInstance();
            resource.ResourceDate = entity.ModifiedAt ?? entity.CreatedAt;
            resource.ResourceId = entity.Id;
            resource.Version = entity.Version;
            this.WriteResource(entity, resource);
            return resource;
        }

        public IList<TResource> Adapt(IEnumerable<TEntity>? entities) => entities?.Select(this.Adapt).ToList() ?? new List<TResource>();

        private TEntity RecoverInstanceAsync(TResource resource)
        {
            if (resource.ResourceId.Equals(default))
            {
                return this.CreateEntityInstance();
            }

            var entity = _service.FindById(resource.ResourceId);
            return entity;
        }

        private TEntity CreateEntityInstance()
        {
            var entityType = typeof(TEntity);
            var instance = (TEntity)Activator.CreateInstance(entityType);
            return instance;
        }

        private TResource CreateResourceInstance()
        {
            var resourceType = typeof(TResource);
            var instance = (TResource)Activator.CreateInstance(resourceType);
            return instance;
        }

        public abstract void WriteEntity(TResource resource, TEntity entity);

        public abstract void WriteResource(TEntity entity, TResource resource);
    }
}
