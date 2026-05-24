using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

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

        public static bool CollectAmmoWithWeapon(this IGameEntity character, int amount)
        {
            if(!character.TryGetValue(GameEntityAPI.Weapon, out IReactiveVariable<IGameEntity> weaponVariable))
                return false;

            IGameEntity weapon = weaponVariable.Value;
            if(weapon == null || !weapon.TryGetValue(GameEntityAPI.Ammo, out IReactiveVariable<int> ammo))
                return false;
            
            ammo.Value += amount;
            return true;
        }
    }
}