using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class RotateUseCase
    {
        public static void RotateStep(this IGameEntity entity, RotateArgs args) => 
            entity.RotateStep(args.Direction, args.DeltaTime);

        public static void RotateStep(this IGameEntity entity, Vector3 direction, float deltaTime)
        {
            if(direction == Vector3.zero)
                return;

            IVariable<Quaternion> rotation = entity.GetValue(GameEntityAPI.Rotation);
            float rotationSpeed = entity.GetValue(GameEntityAPI.RotateSpeed).Value;
            
            direction = new Vector3(direction.x, 0, direction.y).normalized;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rotation.Value = Quaternion.RotateTowards(rotation.Value, targetRotation, rotationSpeed * deltaTime);
        }
    }
}