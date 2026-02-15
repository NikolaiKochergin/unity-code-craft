using System.Collections.Generic;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    public sealed class BulletSystem : MonoBehaviour
    {
        [SerializeField] private Bullet _prefab;
        [SerializeField] private Transform _container;
        [SerializeField, Min(0)] private int _initialCapacity = 10;
        [SerializeField] private BulletViewConfig _configView;
        [SerializeField] private TransformBounds _levelBounds;

        private ComponentPool<ExplosionParticles> _explosionPool;
        private ComponentPool<Bullet> _bulletPool;
        private readonly List<Bullet> _activeBullets = new();

        private void Awake()
        {
            _explosionPool = new ComponentPool<ExplosionParticles>(_configView.ExplosionVFX, _initialCapacity);
            _bulletPool = new ComponentPool<Bullet>(_prefab, _initialCapacity, _container);
        }

        private void FixedUpdate()
        {
            for (int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _activeBullets[i];
                bullet.Move();

                if (!_levelBounds.InBounds(bullet.transform.position))
                    DeactivateBullet(bullet);
            }
        }

        public void Spawn(Vector2 position, Vector2 direction, float speed, int damage, TeamType team)
        {
            if(!_bulletPool.TryGet(out Bullet bullet))
                return;
            
            bullet.Setup(team, position, direction, damage, speed, OnDamageApplied);

            _activeBullets.Add(bullet);
            bullet.gameObject.SetActive(true);
        }

        private void OnDamageApplied(Bullet bullet)
        {
            DeactivateBullet(bullet);

            _explosionPool
                .Get()
                .Play(DeactivateExplosion);
        }

        private void DeactivateBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _activeBullets.Remove(bullet);
            _bulletPool.Return(bullet);
        }

        private void DeactivateExplosion(ExplosionParticles explosion)
        {
            explosion.gameObject.SetActive(false);
            _explosionPool.Return(explosion);
        }
    }
}