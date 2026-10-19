using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Mine : NetworkBehaviour
    {
        [SerializeField] private TargetDetector _targetDetector;
        [SerializeField] private int _damage;
        
        private VfxBus _vfxBus;

        [Inject]
        public void Construct(VfxBus vfxBus) => 
            _vfxBus = vfxBus;

        public override void FixedUpdateNetwork()
        {
            _targetDetector.Scan();
            if (_targetDetector.HasTarget)
                Explode();
        }

        private void Explode()
        {
            foreach (NetworkObject target in _targetDetector.Targets)
            {
                if(target.TryGetBehaviour(out TakeDamageComponent takeDamage))
                    takeDamage.TakeDamage(new TakeDamageArgs(Object.InputAuthority, _damage));
            }
            
            Runner.Despawn(Object);
            _vfxBus.PlayVfx(VfxType.Explode, transform);
        }
    }
}