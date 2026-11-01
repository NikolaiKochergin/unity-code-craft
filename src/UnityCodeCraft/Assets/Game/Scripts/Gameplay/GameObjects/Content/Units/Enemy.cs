using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Enemy : NetworkBehaviour,
        MoveToTargetComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveToTargetComponent _moveToTargetComponent;
        [SerializeField] private EnemyCollisionComponent _enemyCollisionComponent;
        
        private VfxBus _vfxBus;
        private bool _isDestroyed;

        [Inject]
        public void Construct(VfxBus vfxBus)
        {
            _vfxBus = vfxBus;
        }

        public override void Spawned()
        {
            _moveToTargetComponent.SetCondition(this);
        }
        
        public override void FixedUpdateNetwork()
        {
            if(_isDestroyed)
                return;

            if (_healthComponent.IsDead)
            {
                Runner.Despawn(Object);
                _vfxBus.PlayVfx(VfxType.EnemyDie, transform);
                _isDestroyed = true;
                return;
            }
            
            _enemyCollisionComponent.HandleCollision();
        }

        bool MoveToTargetComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;
    }
}