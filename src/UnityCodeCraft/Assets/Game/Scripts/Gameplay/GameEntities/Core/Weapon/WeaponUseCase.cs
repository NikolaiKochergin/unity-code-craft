using Atomic.Entities;

namespace Game.Gameplay
{
    public static class WeaponUseCase
    {
        public static void FireWithWeapon(this IGameEntity entity) =>
            entity
                .GetValue(GameEntityAPI.Weapon).Value
                ?.GetValue(GameEntityAPI.FireCommand).Invoke();

        public static bool CanFireWithWeapon(this IGameEntity entity)
        {
            IGameEntity weapon = entity.GetValue(GameEntityAPI.Weapon).Value;
            return weapon != null && weapon.GetValue(GameEntityAPI.FireCommand).CanInvoke();
        }
    }
}