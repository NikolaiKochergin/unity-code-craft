using Unity.Entities;

namespace Game
{
    public readonly struct EntityBuilder
    {
        public EntityBuilder(IBaker baker, Entity entity)
        {
            Baker = baker;
            Entity = entity;
        }

        public IBaker Baker { get; }
        public Entity Entity { get; }

        public static implicit operator Entity(EntityBuilder builder)
            => builder.Entity;
    }
    
    public static class EntityBuilderExtensions
    {
        public static EntityBuilder Entity(
            this IBaker baker, 
            TransformUsageFlags flags = TransformUsageFlags.None) =>
            new(baker, baker.GetEntity(flags));

        public static EntityBuilder With<T>(this EntityBuilder entity)
            where T : unmanaged, IComponentData
        {
            entity.Baker.AddComponent<T>(entity.Entity);
            return entity;
        }

        public static EntityBuilder With<T>(this EntityBuilder entity, T component)
            where T : unmanaged, IComponentData
        {
            entity.Baker.AddComponent(entity.Entity, component);
            return entity;
        }

        public static EntityBuilder WithEnabled<T>(
            this EntityBuilder entity,
            bool enabled = true)
            where T : unmanaged, IEnableableComponent
        {
            entity.Baker.AddComponent<T>(entity.Entity);
            entity.Baker.SetComponentEnabled<T>(entity.Entity, enabled);
            return entity;
        }
        
        public static EntityBuilder WithEnabled<T>(
            this EntityBuilder entity,
            T component,
            bool enabled = true)
            where T : unmanaged, IComponentData, IEnableableComponent
        {
            entity.Baker.AddComponent(entity.Entity, component);
            entity.Baker.SetComponentEnabled<T>(entity.Entity, enabled);
            return entity;
        }
    }
}