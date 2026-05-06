using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class MoveUseCase
    {
        public static bool IsMoving(this IGameEntity entity) => 
            entity.GetValue(GameEntityAPI.MoveTime).IsPlaying();

        public static void MoveWithAimingOrRotate(this IGameEntity entity, MoveArgs args)
        {
            if (entity.IsAiming())
                entity.MoveAiming(args);
            else
                entity.RotateStep(args.Direction, args.DeltaTime);
        }

        public static void MoveAiming(this IGameEntity entity, MoveArgs args) => 
            entity.MoveAiming(args.Direction);

        public static void MoveAiming(this IGameEntity entity, Vector3 direction) =>
            entity.GetValue(GameEntityAPI.AimDirection).Value =
                Quaternion.Inverse(entity.GetValue(GameEntityAPI.Rotation).Value) *
                direction.normalized;

        public static void MoveStepForward(this IGameEntity entity, float speed, float deltaTime)
        {
            Vector3 direction = entity.GetValue(GameEntityAPI.Rotation).Value * Vector3.forward;
            entity.MoveStep(direction, speed, deltaTime);
        }

        public static void MoveStep(this IGameEntity entity, Vector3 direction, float speed, float deltaTime) => 
            entity.GetValue(GameEntityAPI.Position).Value += direction * speed * deltaTime;
    }
}