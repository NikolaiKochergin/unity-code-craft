using System;
using UnityEngine;

namespace Game
{
    // +
    public sealed class Ship : MonoBehaviour, IDamageable
    {
        public event Action<Ship> OnFire;

        public ShipControllerSO config;

        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Mover Mover { get; private set; }

        [Header("Combat")]
        public Transform firePoint;
        public float bulletSpeed;
        public int bulletDamage;
        private float _fireTime;
        
        private void Awake()
        {
            Health = new Health(config.Health);
            Mover.SetSpeed(config.MoveSpeed);
        }

        private void FixedUpdate() => Mover.FixedUpdate();

        public void Fire()
        {
            float time = Time.time;
            if (time - _fireTime < config.FireCooldown || !Health.IsAlive)
                return;

            this.OnFire?.Invoke(this);
            _fireTime = time;
        }
    }
}