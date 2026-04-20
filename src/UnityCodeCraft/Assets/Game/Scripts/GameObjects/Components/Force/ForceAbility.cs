using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ForceAbility : MonoBehaviour
    {
        [SerializeField] private Transform _applyPoint;
        [SerializeField] private ForceComponent _forceComponent;
        [SerializeField] private AbilityRequestComponent _forceRequest;
        [SerializeField] private CooldownComponent _cooldown;
        [SerializeField] private Optional<DelayComponent> _delay;
        [SerializeField] private Optional<DamageComponent> _damage;
        [SerializeField] private TargetDetector _targetDetector;

        public event Action OnApplied;
        
        private void Awake()
        {
            _forceRequest.SetCondition(() => _cooldown.IsExpired);
            _forceRequest.SetAction(() =>
            {
                if(_delay)
                    _delay.Value.DelayedInvoke(ApplyInvoke);
                else
                    ApplyInvoke();
            });
        }

        public void Apply() => 
            _forceRequest.Require();

        private void ApplyInvoke()
        {
            IReadOnlyList<Transform> targets = _targetDetector.GetTargets();

            foreach (Transform target in targets)
            {
                Vector2 direction = new((target.position - _applyPoint.position).normalized.x, 1);
                _forceComponent.ApplyTo(target, direction);
                if (_damage.Active && target.TryGetComponent(out HealthComponent health))
                    _damage.Value.Apply(health);
            }
            
            _cooldown.Reset();
            OnApplied?.Invoke();
        }
    }
}