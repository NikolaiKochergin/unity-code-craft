using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class BulletCollisionBehaviour : IGameEntityInit, IGameEntityDisable
    {
        private readonly IGameContext _gameContext;
        
        private IValue<int> _damage;
        private TriggerEvents _trigger;
        private IValue<TeamType> _team;
        private IAction _destroyAction;

        public BulletCollisionBehaviour(IGameContext gameContext) => 
            _gameContext = gameContext;

        public void Init(IGameEntity entity)
        {
            _damage = entity.GetValue(GameEntityAPI.Damage);
            _team = entity.GetValue(GameEntityAPI.Team);
            _destroyAction = entity.GetValue(GameEntityAPI.DestroyAction);

            _trigger = entity.GetValue(GameEntityAPI.Trigger);
            _trigger.OnEntered += OnTriggerEnter;
        }

        public void Disable(IGameEntity entity) => 
            _trigger.OnEntered -= OnTriggerEnter;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out IGameEntity target) &&
                _gameContext.TakeDamage(target, _damage.Value, _team.Value))
                _destroyAction.Invoke();
        }
    }
}