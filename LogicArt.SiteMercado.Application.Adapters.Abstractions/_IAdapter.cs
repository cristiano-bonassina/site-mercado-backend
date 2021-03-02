using System.Collections.Generic;

namespace LogicArt.SiteMercado.Application.Adapters.Abstractions
{
    public interface IAdapter<TEntity, TResource>
    {

        TEntity Adapt(TResource? resource);

        IList<TResource> Adapt(IEnumerable<TEntity>? entities);

        TResource? Adapt(TEntity? entity);

    }
}
