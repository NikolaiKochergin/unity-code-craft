using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class DeathVfxComponent : NetworkBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ParticleSpawner _particleSpawner;

        public override void Spawned() => 
            _healthComponent.OnHealthChanged += OnHealthChanged;

        public override void Despawned(NetworkRunner _, bool __) => 
            _healthComponent.OnHealthChanged -= OnHealthChanged;

        private void OnHealthChanged(int previous, int current)
        {
            if(/*previous > 0 && */current <= 0)
                _particleSpawner.Play();
        }
    }
}