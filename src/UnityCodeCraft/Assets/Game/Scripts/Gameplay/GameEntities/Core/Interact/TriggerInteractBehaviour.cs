using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class TriggerInteractBehaviour : IGameEntityInit, IGameEntityDispose
    {
        private IGameEntity _self;
        private TriggerEvents _triggerEvents;

        public void Init(IGameEntity entity)
        {
            _self = entity;
            _triggerEvents = entity.GetValue(GameEntityAPI.Trigger);
            _triggerEvents.OnEntered += OnTriggerEntered;
        }

        public void Dispose(IGameEntity entity)
        {
            _triggerEvents.OnEntered -= OnTriggerEntered;
        }

        private void OnTriggerEntered(Collider collider)
        {
            if (collider.TryGetComponent(out IGameEntity target)) 
                _self.InteractWith(target);
        }
    }
}