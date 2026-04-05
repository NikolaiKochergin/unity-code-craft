using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using SampleGame.Common;

namespace Game.Gameplay
{
    public class EntityWorldSerializer : ISaveSerializer<EntityData[]>
    {
        private readonly EntityWorld _world;
        private readonly List<IComponentSerializer> _componentSerializers;

        public EntityWorldSerializer(EntityWorld world, List<IComponentSerializer> componentSerializers)
        {
            _world = world;
            _componentSerializers = componentSerializers;
        }

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

                foreach (IComponentSerializer serializer in _componentSerializers) 
                    serializer.Deserialize(entity, entityData.Components);
            }
        }

        private JObject ComponentsData(Entity entity)
        {
            JObject componentsData = new();
            foreach (IComponentSerializer serializer in _componentSerializers) 
                serializer.Serialize(entity, componentsData);
            
            return componentsData;
        }
    }

    public struct EntityData
    {
        public int Id;
        public string Name;
        public SerializedVector3 Position;
        public SerializedVector3 Rotation;
        public JObject Components;
    }
}