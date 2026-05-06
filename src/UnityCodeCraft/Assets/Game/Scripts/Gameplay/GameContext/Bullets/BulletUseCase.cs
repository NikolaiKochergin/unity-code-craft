using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class BulletUseCase
    {
        public static IGameEntity SpawnBullet(this IGameEntity weapon, Vector3 position, Quaternion rotation) => 
            SceneEntity.Create(weapon.GetValue(GameEntityAPI.BulletPrefab), position, rotation);

        public static void DespawnBullet(this IGameEntity bullet) => 
            SceneEntity.Destroy(bullet);
    }
}