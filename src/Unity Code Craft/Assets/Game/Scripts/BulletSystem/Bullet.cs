using System;
using UnityEngine;

namespace Game
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private Vector2 _direction;
        [SerializeField] private BulletConfig _config;

        public Vector3 Position => transform.position;
        public TeamType Team => _config.Team;
        
        public event Action<Bullet> OnDamageApplied;

        public void Setup(Vector2 position, Vector2 direction, BulletConfig config)
        {
            _config = config;
            _direction = direction;
            
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
            
            SetLayer(config.Team);
        }

        public void Move()
        {
            Vector3 moveStep = _direction * (_config.Speed * Time.fixedDeltaTime);
            transform.position += moveStep;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(_config.Damage <= 0 || !other.TryGetComponent(out Ship targer))
                return;
            
            targer.TakeDamage(_config.Damage);

            OnDamageApplied?.Invoke(this);
        }

        private void SetLayer(TeamType team) =>
            gameObject.layer = team switch
            {
                TeamType.None => LayerMask.NameToLayer("Default"),
                TeamType.Player => LayerMask.NameToLayer("PlayerBullet"),
                TeamType.Enemy => LayerMask.NameToLayer("EnemyBullet"),
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
            };
    }
}