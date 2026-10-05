using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class Character : NetworkBehaviour,
        MoveComponent.ICondition,
        WeaponComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private WeaponComponent _weaponComponent;

        public override void Spawned()
        {
            _moveComponent.SetCondition(this);
            _weaponComponent.SetCondition(this);
        }

        private void FixedUpdate()
        {
            _weaponComponent.StartFire();
        }

        bool MoveComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;

        bool WeaponComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;
    }
}