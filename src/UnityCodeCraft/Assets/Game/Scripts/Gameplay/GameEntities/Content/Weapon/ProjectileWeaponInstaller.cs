using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class ProjectileWeaponInstaller : WeaponInstaller
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private GameEntity _bulletPrefab;

        public override void Install(IGameEntity weapon)
        {
            base.Install(weapon);
            
            weapon.AddValue(GameEntityAPI.BulletPrefab, _bulletPrefab);
            
            weapon.GetValue(GameEntityAPI.FireCommand).AddAction(() =>
            {
                weapon.SpawnBullet(
                   _firePoint.position,
                   _firePoint.rotation
                );
            });
        }
    }
}