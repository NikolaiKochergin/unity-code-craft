using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class ProjectileWeaponInstaller : WeaponInstaller
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField, Min(0)] private float _fireRate;

        public override void Install(IGameEntity weapon)
        {
            base.Install(weapon);
            
            GameContext gameContext = GameContext.Instance;
            weapon.GetValue(GameEntityAPI.FireCommand).AddAction(() =>
            {
                IGameEntity owner = weapon.GetValue(GameEntityAPI.Owner).Value;
                gameContext.SpawnBullet(
                   _firePoint.position,
                   _firePoint.WithFireRate(_fireRate),
                   owner.GetValue(GameEntityAPI.Team).Value
                );
            });
        }
    }
}