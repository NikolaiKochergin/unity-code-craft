using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class Mine : NetworkBehaviour
    {
        [SerializeField] private ParticleSpawner _particleSpawner;
        [SerializeField] private TargetDetector _targetDetector;
        [SerializeField] private int _damage;
        
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
            _particleSpawner.Play();
        }
    }
}