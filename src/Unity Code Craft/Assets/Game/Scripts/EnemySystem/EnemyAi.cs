using System;
using UnityEngine;

namespace Game
{
    public sealed class EnemyAi : MonoBehaviour
    {
        [SerializeField] private Ship _ship;
        [SerializeField] private float _fireCooldown = 1.25f;
        [SerializeField] private float _stoppingDistance = 0.25f;
        
        private IEnemyDespawner _despawner;
        private IHealth _target;
        private Vector2 _destination;
        
        private float _fireTime;
        private Action<AttackEvent> _onFire;

        public void Setup(IEnemyDespawner despawner,
            IHealth target,
            Vector2 destination, 
            Action<AttackEvent> onFire)
        {
            _onFire = onFire;
            _despawner = despawner;
            _target = target;
            _destination = destination;
            _ship.Health.Restore();
            
            _ship.Attack.OnFire += _onFire;
            _ship.Health.OnDied += OnCharacterDead;
        }

        private void OnCharacterDead()
        {
            _ship.Attack.OnFire -= _onFire;
            _ship.Health.OnDied -= OnCharacterDead;
            _despawner.Despawn(this);
        }

        private void FixedUpdate()
        {
            if (!_ship.Health.IsAlive || _target == null || !_target.IsAlive)
                return;

            Vector2 distance = _destination - (Vector2) transform.position;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;
            
            if (isNotReached)
            {
                _ship.Mover.SetDirection(distance.normalized);
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