using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class Turret : NetworkBehaviour,
        WeaponComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private WeaponComponent _weaponComponent;
        [SerializeField] private TargetDetector _targetDetector;
        
        public override void Spawned() => 
            _weaponComponent.SetCondition(this);
        
        public override void FixedUpdateNetwork()
        {
            if(_healthComponent.IsDead)
                return;
            
            _targetDetector.Scan();
            if(!_targetDetector.TryGetNearestTarget(out NetworkObject target))
                return;
            
            transform.LookAt(target.transform);
            _weaponComponent.StartFire();
        }

        bool WeaponComponent.ICondition.IsMet() => 
            !_healthComponent.IsDead;
    }
}