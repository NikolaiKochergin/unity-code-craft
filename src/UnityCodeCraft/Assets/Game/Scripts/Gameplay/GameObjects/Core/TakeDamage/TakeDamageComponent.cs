using System;
using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class TakeDamageComponent : NetworkBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;

        [Networked]
        private TakeDamageArgs LastDamage { get; set; }
        
        [Networked]
        private ushort DamageCount { get; set; }
        
        private ushort _localDamageCount;
        
        public event Action<TakeDamageArgs> OnDamageTaken;

        public void TakeDamage(TakeDamageArgs args)
        {
            if(args.Damage <= 0 || _healthComponent.IsDead)
                return;
            
            _healthComponent.Decrement(args.Damage);
            LastDamage = args;
            DamageCount++;

            if (_healthComponent.IsDead)
                NotifyAboutKill(args.Instigator);
        }

        private void NotifyAboutKill(NetworkId killer) =>
            Runner
                .GetBehaviour<KillNotificator>()
                .NotifyAboutKill(killer, Object.Id);

        public override void Spawned() => 
            _localDamageCount = DamageCount;

        public override void Render()
        {
            while (_localDamageCount < DamageCount)
            {
                OnDamageTaken?.Invoke(LastDamage);
                _localDamageCount++;
            }
        }
    }
}