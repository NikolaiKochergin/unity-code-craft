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

        public void Start() => 
            _ship.OnDied += OnCharacterDead;

        private void OnDestroy() => 
            _ship.OnDied -= OnCharacterDead;

        public void Enable() => 
            gameObject.SetActive(true);
        
        public void Disable() =>
            gameObject.SetActive(false);
        
        public void SetTarget(Ship target) =>
            _target = target;

        public void SetPosition(Vector3 nextSpawnPosition) =>
            _ship.Position = nextSpawnPosition;

        public void SetDestination(Vector3 destination) =>
            _destination = destination;

        private void OnCharacterDead() => 
            OnDied?.Invoke(this);

        private void FixedUpdate()
        {
            if (!_ship.IsAlive || _target == null || !_target.IsAlive)
                return;

            Vector2 distance = _destination - (Vector2)transform.position;
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