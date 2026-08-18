using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    public static class ProjectileUseCase
    {
        public static void SpawnProjectile(
            ref  EntityCommandBuffer ecb,
            ProjectilePrefab projectilePrefab,
            LocalTransform transform,
            FireOffset fireOffset,
            int damage,
            RefRO<Team> team,
            Entity target)
        {
            Entity projectile = ecb.Instantiate(projectilePrefab.Value);

            float3 spawnPosition = FireUseCase.GetFirePoint(transform, fireOffset);
            quaternion spawnRotation = transform.Rotation;
            
            ecb.SetComponent(projectile, LocalTransform.FromPositionRotation(spawnPosition, spawnRotation));
            ecb.SetComponent(projectile, team.ValueRO);
            ecb.SetComponent(projectile, new TargetEntity { Value = target });
            ecb.SetComponent(projectile, new Damage { Value = damage });
        }
    }
}