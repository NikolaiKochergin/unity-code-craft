using System;
using UnityEngine;

namespace Game
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private Ship _ship;
        [SerializeField] private float _fireCooldown = 1.25f;
        [SerializeField] private float _stoppingDistance = 0.25f;
        
        private Ship _target;
        private Vector2 _destination;
        
        private float _fireTime;

        public event Action<Enemy> OnDied;

        public void Setup(BulletSystem bulletSystem, Ship target, Vector2 destination)
        {
            _target = target;
            _destination = destination;
            
            _ship.Setup(bulletSystem);
            _ship.RestoreHealth();
            
            _ship.OnDied += OnCharacterDead;
        }

        private void OnCharacterDead()
        {
            _ship.OnDied -= OnCharacterDead;
            OnDied?.Invoke(this);
        }

        private void FixedUpdate()
        {
            if (!_ship.IsAlive || _target == null || !_target.IsAlive)
                return;

            Vector2 distance = _destination - (Vector2) transform.position;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;
            
            if (isNotReached)
            {
                _ship.Direction = distance.normalized;
            }
            else
            {
                float time = Time.time;
                if (time - _fireTime >= _fireCooldown)
                {
                    _ship.FireAt(_target.Position);
                    _fireTime = time;
                }
            }
        }
    }
}