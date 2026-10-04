using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class Enemy : NetworkBehaviour,
        MoveComponent.ICondition,
        MoveToTargetComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private MoveToTargetComponent _moveToTargetComponent;

        public override void Spawned()
        {
            _moveComponent.SetCondition(this);
            _moveToTargetComponent.SetCondition(this);
        }

        bool MoveComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;

        public bool IsMet() => 
            _healthComponent.IsAlive;
    }
}