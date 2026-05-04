using Atomic.Entities;

namespace Game.Gameplay
{
    public static class AimUseCase
    {
        public static bool IsAiming(this IGameEntity entity) =>
            entity.GetValue(GameEntityAPI.AimTime).IsPlaying();
    }
}