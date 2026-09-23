using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class HealthAnimComponent : NetworkBehaviour
    {
        private static readonly int IsDead = Animator.StringToHash(nameof(IsDead));
        
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private Animator _animator;

        public override void Spawned()
        {
            _healthComponent.OnHealthChanged += OnHealthChanged;
            OnHealthChanged();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _healthComponent.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if(_healthComponent.IsDead)
                _animator.SetTrigger(IsDead);
        }
    }
}