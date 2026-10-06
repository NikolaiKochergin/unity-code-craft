using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class Enemy : NetworkBehaviour,
        MoveComponent.ICondition,
        MoveToTargetComponent.ICondition,
        WeaponComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private MoveToTargetComponent _moveToTargetComponent;
        [SerializeField] private WeaponComponent _weaponComponent;
        [SerializeField] private TargetDetector _targetDetector;

        public override void Spawned()
        {
            _moveComponent.SetCondition(this);
            _moveToTargetComponent.SetCondition(this);
            _weaponComponent.SetCondition(this);

            _healthComponent.OnHealthChanged += OnHealthChanged;
        }

        public override void Despawned(NetworkRunner _, bool __) => 
            _healthComponent.OnHealthChanged -= OnHealthChanged;

        private void OnHealthChanged(int _, int __)
        {
            if(_healthComponent.IsDead)
                Runner.Despawn(Object);
        }
        
        private void FixedUpdate()
        {
            _weaponComponent.StartFire();
        }

        bool MoveComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;

        bool MoveToTargetComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;

        bool WeaponComponent.ICondition.IsMet()
        {
            _targetDetector.Scan();
            return _healthComponent.IsAlive && _targetDetector.HasTarget;
        }
    }
}