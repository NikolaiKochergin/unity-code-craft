using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class DeathVfxComponent : NetworkBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ParticleSpawner _particleSpawner;

        public override void Despawned(NetworkRunner _, bool __)
        {
            if(_healthComponent.Current <= 0)
                _particleSpawner.Play();
        }
    }
}