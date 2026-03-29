using System;
using System.Collections.Generic;
using Modules.Entities;
using SampleGame.Common;
using SampleGame.Gameplay;
using Component = SampleGame.Gameplay.Component;

namespace Game.Gameplay
{
    public class EntityWorldSerializer : ISaveSerializer<EntityData[]>
    {
        private readonly EntityWorld _world;

        public EntityWorldSerializer(EntityWorld world) => 
            _world = world;

        public string Key => "World";
        
        public EntityData[] Serialize()
        {
            IReadOnlyCollection<Entity> entities = _world.GetAll();
            EntityData[] result = new EntityData[entities.Count];
            int index = 0;

            foreach (Entity entity in entities)
            {
                result[index++] = new EntityData
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Position = entity.transform.position,
                    Rotation = entity.transform.rotation,
                    Components = ComponentsData(entity),
                };
            }
            return result;
        }

        private static ComponentData[] ComponentsData(Entity entity)
        {
            Component[] components = entity.GetComponents<Component>();
            ComponentData[] componentsData = new ComponentData[components.Length];
            int index = 0;
            foreach (Component component in components) 
                componentsData[index++] = component.AsData();
            
            return componentsData;
        }

        public void Deserialize(EntityData[] data)
        {
            foreach (EntityData entityData in data)
            {
                if (_world.TryGet(entityData.Id, out Entity entity))
                {
                    entity.transform.position = entityData.Position;
                    entity.transform.rotation = entityData.Rotation;
                }
                else
                {
                    _world.Spawn(
                        entityData.Name, 
                        entityData.Position, 
                        entityData.Rotation,
                        entityData.Id);
                }

                foreach (ComponentData componentData in entityData.Components)
                {
                    Component[] components = entity.GetComponents<Component>();
                    foreach (Component component in components)
                        if(component.GetType().Name == componentData.Name)
                            component.Restore(componentData);
                }
            }
        }
        
        
    }

    public struct EntityData
    {
        public int Id;
        public string Name;
        public SerializedVector3 Position;
        public SerializedVector3 Rotation;
        public ComponentData[] Components;
    }
    
    public struct ComponentData
    {
        public string Name;
        public string Value;
    }
}