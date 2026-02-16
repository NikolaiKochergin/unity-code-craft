using UnityEngine;

namespace Game
{
    // +
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private Ship _ship;
        
        [Header("Enemy")]
        public Ship target;
        public Vector2 destination;

        [SerializeField]
        private float _fireCooldown = 1.25f;

        [SerializeField]
        private float _stoppingDistance = 0.25f;

        private float _fireTime;

        private IEnemyDespawner _despawner;
        
        public Ship Ship => _ship;

        public void SetDespawner(IEnemyDespawner despawner) => _despawner = despawner;

        private void OnEnable() => _ship.Health.OnDied += OnCharacterDead;

        private void OnDisable() => _ship.Health.OnDied -= OnCharacterDead;

        private void OnCharacterDead() => _despawner.Despawn(this);

        private void FixedUpdate()
        {
            if (!_ship.Health.IsAlive || target == null || !target.Health.IsAlive)
                return;

            Vector2 distance = destination - (Vector2) this.transform.position;
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
                    _ship.Fire();
                    _fireTime = time;
                }
            }
        }
    }
}