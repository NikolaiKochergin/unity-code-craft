using System;
using UnityEngine;

namespace Game
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private TeamType _team = TeamType.None;
        [SerializeField] private Vector2 _direction;
        [SerializeField] public int _damage;
        [SerializeField] public float _speed;
        
        [SerializeField] private GameObject blueVFX;
        [SerializeField] private GameObject redVFX;
        
        private Action<Bullet> _onDamageApplied;

        public void Setup(
            TeamType team,
            Vector2 position,
            Vector2 direction, 
            int damage, 
            float speed, 
            Action<Bullet> onDamageApplied)
        {
            _team = team;
            _direction = direction;
            _damage = damage;
            _speed = speed;
            _onDamageApplied = onDamageApplied;
            
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
            
            SetLayer(_team);
            SetVisual(_team);
        }

        public void Move()
        {
            Vector3 moveStep = _direction * (_speed * Time.fixedDeltaTime);
            transform.position += moveStep;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(_damage <= 0 || !other.TryGetComponent(out IDamageable ship))
                return;
            
            ship.Health.TakeDamage(_damage);

            _onDamageApplied?.Invoke(this);
            _onDamageApplied = null;
        }

        private void SetLayer(TeamType team) =>
            gameObject.layer = team switch
            {
                TeamType.None => LayerMask.NameToLayer("Default"),
                TeamType.Player => LayerMask.NameToLayer("PlayerBullet"),
                TeamType.Enemy => LayerMask.NameToLayer("EnemyBullet"),
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
            };

        private void SetVisual(TeamType team)
        {
            if (team == TeamType.Player)
            {
                blueVFX.SetActive(true);
                redVFX.SetActive(false);
            }
            else
            {
                blueVFX.SetActive(false);
                redVFX.SetActive(true);
            }
        }
    }
}