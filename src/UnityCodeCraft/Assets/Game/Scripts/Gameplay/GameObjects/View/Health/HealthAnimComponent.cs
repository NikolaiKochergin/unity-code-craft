using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class HealthAnimComponent : NetworkBehaviour
    {
        private static readonly int IsDead = Animator.StringToHash(nameof(IsDead));
        
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private Animator _animator;

        public override void Spawned() => 
            _healthComponent.OnHealthChanged += OnHealthChanged;

        public override void Despawned(NetworkRunner _, bool __) => 
            _healthComponent.OnHealthChanged -= OnHealthChanged;

        private void OnHealthChanged(int _, int __) => 
            _animator.SetBool(IsDead, _healthComponent.IsDead);
    }
}