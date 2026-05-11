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

        public static void Punch(this IGameEntity entity)
        {
            Vector3 position = entity.GetValue(GameEntityAPI.Position).Value;
            float radius = entity.GetValue(GameEntityAPI.FistSize).Value;
            Collider[] results = entity.GetValue(GameEntityAPI.HitResult);
            int damage = entity.GetValue(GameEntityAPI.Damage).Value;
            TeamType instigator = entity.GetValue(GameEntityAPI.Team).Value;
            
            int size = Physics.OverlapSphereNonAlloc(position, radius, results);
            for (int i = 0; i < size; i++)
                if (results[i].TryGetComponent(out IGameEntity target) && target.HasTag(GameEntityAPI.CharacterTag))
                    if(GameContext.Instance.TakeDamage(target, damage, instigator))
                        break;
        }
    }
}