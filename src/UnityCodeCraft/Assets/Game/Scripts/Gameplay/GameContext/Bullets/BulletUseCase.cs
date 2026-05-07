using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class BulletUseCase
    {
        public static IGameEntity SpawnBullet(this IGameEntity weapon, Vector3 position, Quaternion rotation, TeamType team)
        {
            GameEntity bullet = SceneEntity.Create(weapon.GetValue(GameEntityAPI.BulletPrefab), position, rotation);
            bullet.GetValue(GameEntityAPI.Team).Value = team;
            return bullet;
        }

        public static void DespawnBullet(this IGameEntity bullet) => 
            SceneEntity.Destroy(bullet);
    }
}