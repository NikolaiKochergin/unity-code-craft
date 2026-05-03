using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class AimUseCase
    {
        public static bool IsAiming(this IGameEntity entity) =>
            entity.GetValue(GameEntityAPI.AimDirection).Value != Vector3.zero;
    }
}