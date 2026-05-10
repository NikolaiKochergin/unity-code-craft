using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class DetectCharacterBehaviour : IGameEntityInit, IGameEntityDispose
    {
        private TriggerEvents _triggerEvents;
        private GameEntity[] _zombies;

        public void Init(IGameEntity entity)
        {
            _triggerEvents = entity.GetValue(GameEntityAPI.Trigger);
            _zombies = entity.GetValue(GameEntityAPI.Zombies);

            _triggerEvents.OnEntered += OnTriggerEnter;
            _triggerEvents.OnExited += OnTriggerExit;
        }

        public void Dispose(IGameEntity entity)
        {
            _triggerEvents.OnEntered -= OnTriggerEnter;
            _triggerEvents.OnExited -= OnTriggerExit;
        }

        private void OnTriggerEnter(Collider col)
        {

            if (col.TryGetComponent(out IGameEntity entity) && entity.HasTag(GameEntityAPI.CharacterTag))
                foreach (GameEntity zombie in _zombies)
                    zombie.GetValue(GameEntityAPI.Target).Value = entity;
        }

        private void OnTriggerExit(Collider col)
        {
            if(col.TryGetComponent(out IGameEntity entity) && entity.HasTag(GameEntityAPI.CharacterTag))
                foreach (GameEntity zombie in _zombies)
                    zombie.GetValue(GameEntityAPI.Target).Value = null;
        }
    }
}