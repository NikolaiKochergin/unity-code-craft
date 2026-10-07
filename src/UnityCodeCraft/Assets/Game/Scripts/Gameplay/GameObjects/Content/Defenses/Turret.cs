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
        
        public void SetupInstigator(NetworkId characterId)
        {
            PlayerRef player = Runner.FindObject(characterId).InputAuthority;
            Object.AssignInputAuthority(player);
        }

        public override void Spawned() => 
            _weaponComponent.SetCondition(this);
        
        private void FixedUpdate() => 
            _weaponComponent.StartFire();

        bool WeaponComponent.ICondition.IsMet()
        {
            if(_healthComponent.IsDead)
                return false;
            
            _targetDetector.Scan();
            if(!_targetDetector.TryGetNearestTarget(out NetworkObject target))
                return false;
            
            transform.LookAt(target.transform);
            
            return true;
        }
    }
}