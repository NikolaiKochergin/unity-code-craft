using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class FireAnimComponent : NetworkBehaviour
    {
        private static readonly int Fire = Animator.StringToHash(nameof(Fire));

        [SerializeField] private WeaponComponent _weaponComponent;
        [SerializeField] private Animator _animator;

        public override void Spawned() => 
            _weaponComponent.OnFireStarted += OnFireStarted;

        public override void Despawned(NetworkRunner _, bool __) => 
            _weaponComponent.OnFireStarted -= OnFireStarted;

        private void OnFireStarted() => 
            _animator.SetTrigger(Fire);
    }
}