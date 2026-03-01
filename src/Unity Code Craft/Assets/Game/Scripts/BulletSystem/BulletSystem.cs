using System.Collections.Generic;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    public sealed class BulletSystem : MonoBehaviour
    {
        [SerializeField] private TransformBounds _levelBounds;
        [SerializeField] private BulletPool _bulletPool;
        
        private readonly List<Bullet> _activeBullets = new();

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

        public void Spawn(Vector2 position, Vector2 direction, BulletConfig config)
        {
            if(!_bulletPool.TryGet(out Bullet bullet))
                return;
            
            bullet.Setup(position, direction, config);
            bullet.OnDamageApplied += OnDamageApplied;

            _activeBullets.Add(bullet);
            bullet.Enable();
        }

        private void OnDamageApplied(Bullet bullet)
        {
            bullet.OnDamageApplied -= OnDamageApplied;
            
            DeactivateBullet(bullet);
        }

        private void DeactivateBullet(Bullet bullet)
        {
            bullet.Disable();
            _activeBullets.Remove(bullet);
            _bulletPool.Return(bullet);
        }
    }
}