using System.Linq;
using LogicArt.Arch.Application.Bus;
using LogicArt.Arch.Application.Bus.Abstractions;
using LogicArt.Arch.Application.Events;
using LogicArt.Arch.Domain.Entities.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Application.Events
{
    public class EventSchemaProvider : IEventSchemaProvider
    {
        public IEventSchema GetSchema()
        {

            var schema = new EventSchema();

            var entityTypes = typeof(IEntitiesAssembly).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract && !x.IsInterface && typeof(IEntity).IsAssignableFrom(x))
                .ToList();
            foreach (var entityType in entityTypes)
            {
                var @event = new EventBuilder()
                    .ForEntity(entityType)
                    .WithEvent<AuditEvent>()
                    .WithEvent<ValidationEvent>()
                    .WithEvent<VersioningEvent>()
                    .Build();
                schema.Add(@event);
            }

            var imageProcessorEvent = new EventBuilder()
                    .ForEntity<Product>()
                    .WithEvent<ImageProcessorEvent>()
                    .Build();
            schema.Add(imageProcessorEvent);

            return schema;

        }
    }
}
