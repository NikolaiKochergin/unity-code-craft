using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class PortalHealthPresenter : NetworkBehaviour
    {
        private SmoothHealthBar _healthView;
        private HealthComponent _health;

        [Inject]
        public void Construct(
            [Inject(Id = Tags.Portal)] NetworkObject portal,
            [Inject(Id = Tags.Portal)] SmoothHealthBar healthView)
        {
            _health = portal.GetBehaviour<HealthComponent>();
            _healthView = healthView;
        }

        public override void Spawned()
        {
            UpdateHealthBar(_health.Current);
            _health.OnHealthChanged += OnHealthChanged;
        }

        public override void Despawned(NetworkRunner _, bool __) => 
            _health.OnHealthChanged -= OnHealthChanged;

        private void OnHealthChanged(int _, int __) => 
            UpdateHealthBar(_health.Current);

        private void UpdateHealthBar(int current)
        {
            float healthNormalized = Mathf.Clamp01((float)current / _health.Max);
            _healthView.SetCaption($"{current}/{_health.Max}");
            _healthView.Set(healthNormalized, true);
        }
    }
}