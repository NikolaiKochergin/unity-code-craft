using System;
using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class WeaponComponent : NetworkBehaviour
    {
        public interface ICondition
        {
            bool IsMet();
        }
        
        [Networked, UnitySerializeField]
        public Weapon Current { get; private set; }

        [SerializeField] private float _meleeFireDelay = 0.25f;
        [SerializeField] private float _rangeFireDelay = 0.2f;
        
        [Networked]
        private TickTimer _delayTimestamp { get; set; }
        
        [Networked]
        private ushort _fireStartedEvents { get; set; }

        private ushort _localFireStartedEvents;
        
        private ICondition _condition;

        public bool IsFireStarted => _delayTimestamp.IsRunning(Runner);
        
        public event Action OnFireStarted;

        public void SetCondition(ICondition condition) => _condition = condition;

        public override void Spawned() => _localFireStartedEvents = _fireStartedEvents;

        public override void FixedUpdateNetwork()
        {
            if (!_delayTimestamp.Expired(Runner) || !CanFire()) 
                return;
            
            Current.Fire();
            _delayTimestamp = default;
        }

        public override void Render()
        {
            while (_localFireStartedEvents < _fireStartedEvents)
            {
                OnFireStarted?.Invoke();
                _localFireStartedEvents++;
            }
        }

        public void StartFire()
        {
            if (!_delayTimestamp.IsRunning && CanFire())
            {
                _delayTimestamp = TickTimer.CreateFromSeconds(Runner, GetDelay());
                _fireStartedEvents++;
            }
        }

        public bool CanFire()
        {
            Weapon current = Current;
            return current != null && current.CanFire() && (_condition == null || _condition.IsMet());
        }

        private float GetDelay() =>
            Current switch
            {
                MeleeWeapon => _meleeFireDelay,
                ProjectileWeapon => _rangeFireDelay,
                _ => throw new ArgumentOutOfRangeException(nameof(Current))
            };
    }
}