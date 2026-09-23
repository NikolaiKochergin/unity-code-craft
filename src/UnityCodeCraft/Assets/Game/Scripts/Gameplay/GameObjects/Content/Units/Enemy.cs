using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class Enemy : NetworkBehaviour,
        MoveComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;

        public override void Spawned() => 
            _moveComponent.SetCondition(this);

        bool MoveComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;
    }
}