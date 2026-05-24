using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class BulletUseCase
    {
        public static IGameEntity SpawnBullet(this IGameContext gameContext,Vector3 position,
            Quaternion rotation, TeamType team)
        {
            IEntityPool<IGameEntity> bulletPool = gameContext.GetValue(GameContextAPI.BulletPool);
            IGameEntity bullet = bulletPool.Rent();
            bullet.GetValue(GameEntityAPI.Position).Value = position;
            bullet.GetValue(GameEntityAPI.Rotation).Value = rotation;
            bullet.GetValue(GameEntityAPI.Team).Value = team;
            bullet.GetValue(GameEntityAPI.RespawnCommand).Invoke();
            return bullet;
        }

        public static void DespawnBullet(this IGameContext gameContext, IGameEntity bullet) => 
            gameContext.GetValue(GameContextAPI.BulletPool).Return(bullet);

        public static Quaternion WithFireRate(this Transform firePoint, float rate) => 
            firePoint.rotation * Quaternion.Euler(0f, Random.Range(-rate, rate) / 2f, 0f);
    }
}