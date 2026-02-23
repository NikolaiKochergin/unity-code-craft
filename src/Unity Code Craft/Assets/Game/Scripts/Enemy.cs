using System;
using UnityEngine;

namespace Game
{
    // +
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private Ship _ship;
        [SerializeField] private float _fireCooldown = 1.25f;
        [SerializeField] private float _stoppingDistance = 0.25f;
        
        private IEnemyDespawner _despawner;
        private IHealth _target;
        private Vector2 _destination;
        
        private float _fireTime;
        
        public event Action<AttackEvent> OnFire
        {
            add => _ship.Attack.OnFire += value;
            remove => _ship.Attack.OnFire -= value;
        }
        
        public void Setup(
            IEnemyDespawner despawner, 
            IHealth target, 
            Vector2 destination)
        {
            _despawner = despawner;
            _target = target;
            _destination = destination;
            _ship.Health.Restore();
        }

        private void OnEnable() => _ship.Health.OnDied += OnCharacterDead;

        private void OnDisable() => _ship.Health.OnDied -= OnCharacterDead;

        private void OnCharacterDead() => _despawner.Despawn(this);

        private void FixedUpdate()
        {
            if (!_ship.Health.IsAlive || _target == null || !_target.IsAlive)
                return;

            Vector2 distance = _destination - (Vector2) transform.position;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;
            
            if (isNotReached)
            {
                _ship.Mover.MoveStep(distance.normalized);
            }
            else
            {
                float time = Time.time;
                if (time - _fireTime >= _fireCooldown)
                {
                    _ship.Attack.Fire();
                    _fireTime = time;
                }
            }
        }
    }
}