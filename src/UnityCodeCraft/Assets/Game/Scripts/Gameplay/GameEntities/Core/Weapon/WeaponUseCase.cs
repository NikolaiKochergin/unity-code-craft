using Atomic.Entities;

namespace Game.Gameplay
{
    public static class WeaponUseCase
    {
        public static void FireWithWeapon(this IGameEntity entity) =>
            entity
                .GetValue(GameEntityAPI.Weapon).Value
                ?.GetValue(GameEntityAPI.FireCommand).Invoke();
    }
}