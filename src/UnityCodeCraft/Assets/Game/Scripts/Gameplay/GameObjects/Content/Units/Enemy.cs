using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Enemy : NetworkBehaviour,
        MoveComponent.ICondition,
        MoveToTargetComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private MoveToTargetComponent _moveToTargetComponent;
        
        private Transform _target;

        [Inject]
        public void Construct(Portal portal) => 
            _target = portal.transform;

        public override void Spawned()
        {
            _moveComponent.SetCondition(this);
            _moveToTargetComponent.SetCondition(this);
            _moveToTargetComponent.SetTarget(_target);
        }

        bool MoveComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;

        public bool IsMet() => 
            _healthComponent.IsAlive;
    }
}