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
                    Components = SerializeComponents(entity),
                };
            }
            return result;
        }

        public void Deserialize(EntityData[] data)
        {
            _world.DestroyAll();

            foreach (EntityData entityData in data)
                _world.Spawn(
                    entityData.Name,
                    entityData.Position,
                    entityData.Rotation,
                    entityData.Id);
            
            foreach (EntityData entityData in data)
            {
                if (!_world.TryGet(entityData.Id, out Entity entity)) 
                    continue;
                
                entity.transform.position = entityData.Position;
                entity.transform.rotation = entityData.Rotation;
                DeserializeComponents(entity, entityData);
            }
        }

        private JObject SerializeComponents(Entity entity)
        {
            JObject componentsData = new();
            foreach (IComponentSerializer serializer in _componentSerializers) 
                serializer.Serialize(entity, componentsData);
            
            return componentsData;
        }

        private void DeserializeComponents(Entity entity, EntityData entityData)
        {
            foreach (IComponentSerializer serializer in _componentSerializers) 
                serializer.Deserialize(entity, entityData.Components);
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