using System;
using UnityEngine;

namespace Game
{
    public class FireComponent : MonoBehaviour
    {
        [SerializeField] private BulletSystem _bulletSystem;
        [SerializeField] private Transform _firePoint;

        public Transform Point => _firePoint;

        private AttackConfig _config;
        private float _fireTime;

        public event Action OnFire;

        public void Setup(AttackConfig config) => 
            _config = config;

        public void Setup(BulletSystem bulletSystem) => 
            _bulletSystem = bulletSystem;

        public void Fire(Vector2 direction)
        {
            float time = Time.time;
            if (time - _fireTime < _config.FireCooldown)
                return;

            _bulletSystem.Spawn(
                _firePoint.position,
                direction,
                _config.Bullet
            );
            OnFire?.Invoke();
            _fireTime = time;
        }
    }
}