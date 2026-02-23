using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CombatComponent
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private int _bulletDamage;
        
        private float _fireTime;

        public event Action OnFire;
        
        public void Fire()
        {
            float time = Time.time;
            if (time - _fireTime < config.FireCooldown || !Health.IsAlive)
                return;

            OnFire?.Invoke();
            _fireTime = time;
        }
    }
}