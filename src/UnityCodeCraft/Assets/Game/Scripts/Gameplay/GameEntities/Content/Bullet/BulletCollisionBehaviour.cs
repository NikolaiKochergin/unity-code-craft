using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class BulletCollisionBehaviour : IGameEntityInit, IGameEntityDispose
    {
        private readonly IGameContext _gameContext;
        private readonly int _damage;
        private readonly TriggerEvents _triggerEvents;
        
        private IValue<TeamType> _team;
        private IAction _destroyAction;

        public BulletCollisionBehaviour(IGameContext gameContext, int damage, TriggerEvents triggerEvents)
        {
            _gameContext = gameContext;
            _damage = damage;
            _triggerEvents = triggerEvents;
        }

        public void Init(IGameEntity entity)
        {
            _team = entity.GetValue(GameEntityAPI.Team);
            _destroyAction = entity.GetValue(GameEntityAPI.DestroyAction);
            _triggerEvents.OnEntered += OnTriggerEnter;
        }

        public void Dispose(IGameEntity entity)
        {
            _triggerEvents.OnEntered -= OnTriggerEnter;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out IGameEntity target) &&
                _gameContext.TakeDamage(target, _damage, _team.Value))
                _destroyAction.Invoke();
        }
    }
}