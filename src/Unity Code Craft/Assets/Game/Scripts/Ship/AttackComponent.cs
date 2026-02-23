using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AttackComponent : IAttack
    {
        [SerializeField] private Transform _firePoint;
        
        private AttackConfig _config;
        private float _fireTime;
        
        public event Action<AttackEvent> OnFire;
        private Func<bool> _attackCondition = () => true;

        public void Setup(AttackConfig config, Func<bool> attackCondition)
        {
            _config = config;
            _attackCondition = attackCondition;
        }

        public void Fire()
        {
            float time = Time.time;
            if (time - _fireTime < _config.FireCooldown || !_attackCondition())
                return;

            OnFire?.Invoke(new AttackEvent(_firePoint, _config.BulletSpeed, _config.BulletDamage));
            _fireTime = time;
        }
    }

    public struct AttackEvent
    {
        public readonly Transform FirePoint;
        public readonly float Speed;
        public readonly int Damage;

        public AttackEvent(Transform firePoint, float speed, int damage)
        {
            FirePoint = firePoint;
            Speed = speed;
            Damage = damage;
        }
    }
}