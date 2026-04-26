using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(TargetDetector))]
    public class FireComponent : MonoBehaviour
    {
        [SerializeField] private TargetDetector _targetDetector;
        [SerializeField] private bool _require;
        [SerializeField] private Vector2 _force;
        [SerializeField, Min(0)] private float _delay;
        [SerializeField, Min(0)] private float _cooldown;
        
        private Func<bool> _condition;
        private float _currentTime;

        public event Action OnFire;

        public void SetCondition(Func<bool> condition) => _condition = condition;

        public void Fire() => _require = true;
        
        private void FixedUpdate()
        {
            if (_require && IsExpired() && (_condition == null || _condition.Invoke()))
            {
                Invoke(nameof(FireInternal), _delay);
                _currentTime = Time.time;
            }
            
            _require = false;
        }
        
        private bool IsExpired() => 
            Time.time - _currentTime > _cooldown;

        private void FireInternal()
        {
            IReadOnlyList<GameObject> targets = _targetDetector.GetTargets();

            foreach (GameObject target in targets)
            {
                if (!target.TryGetComponent(out Rigidbody2D rigidbody)) 
                    continue;
                
                Vector2 force = new(
                    target.transform.position.x - transform.position.x < 0 ? -_force.x : _force.x,
                    _force.y);

                rigidbody.AddForce(force, ForceMode2D.Impulse);
            }
            OnFire?.Invoke();
        }
    }
}