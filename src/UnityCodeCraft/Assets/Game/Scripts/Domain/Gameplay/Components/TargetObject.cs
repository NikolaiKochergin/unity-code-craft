using Game.Gameplay;
using Modules.Entities;
using UnityEngine;
using Zenject;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class TargetObject : MonoBehaviour, IComponentSerializer<int>
    {
        ///Variable
        [field: SerializeField]
        public Entity Value { get; set; }

        private EntityWorld _world;

        [Inject]
        public void Construct(EntityWorld world) => 
            _world = world;

        public string Key => nameof(TargetObject);

        public int Serialize() => Value ? Value.Id : -1;

        public void Deserialize(int data)
        {
            if(_world.TryGet(data, out Entity entity))
                Value = entity;
        }
    }
}