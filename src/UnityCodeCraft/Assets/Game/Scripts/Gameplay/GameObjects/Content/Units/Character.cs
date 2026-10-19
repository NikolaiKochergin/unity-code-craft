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
        [SerializeField] private TargetDetector _targetDetector;
        
        public override void Spawned()
        {
            _moveComponent.SetCondition(this);
            _weaponComponent.SetCondition(this);
        }

        public override void FixedUpdateNetwork()
        {
            if(_healthComponent.IsDead || _moveComponent.IsMoving)
                return;
            
            _targetDetector.Scan();
            if(!_targetDetector.TryGetNearestTarget(out NetworkObject target))
                return;
            
            transform.LookAt(target.transform);
            _weaponComponent.StartFire();
        }

        bool MoveComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;

        bool WeaponComponent.ICondition.IsMet() => 
            !_healthComponent.IsDead && !_moveComponent.IsMoving;
    }
}