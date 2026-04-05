using Modules.Entities;
using SampleGame.Gameplay;

namespace Game.Gameplay
{
    public class TargetObjectComponentSerializer : IComponentSerializer<TargetObject, int>
    {
        private readonly EntityWorld _world;

        public TargetObjectComponentSerializer(EntityWorld world) => 
            _world = world;

        public int Serialize(TargetObject component) => 
            component.Value ? component.Value.Id : -1;

        public void Deserialize(TargetObject component, int data)
        {
            if(_world.TryGet(data, out Entity entity))
                component.Value = entity;
        }
    }
}