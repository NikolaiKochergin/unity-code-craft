using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class WeaponInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField] private GameEntity _owner;
        [SerializeField] private Optional<ReactiveVariable<int>> _ammo;
        [SerializeField] private Optional<Cooldown> _cooldown;
        
        public override void Install(IGameEntity weapon)
        {
            weapon.AddValue(GameEntityAPI.Owner, new Variable<IGameEntity>(_owner));
            weapon.AddValue(GameEntityAPI.FireCommand, new Command());
            
            if (_ammo)
            {
                IReactiveVariable<int> ammo = _ammo.Value;
                weapon.AddValue(GameEntityAPI.Ammo, ammo);
                weapon.GetValue(GameEntityAPI.FireCommand).AddCondition(() => ammo.Value > 0);
                weapon.GetValue(GameEntityAPI.FireCommand).AddAction(() => ammo.Value--);
            }

            if (_cooldown)
            {
                ICooldown cooldown = _cooldown.Value;
                weapon.AddValue(GameEntityAPI.FireCooldown, cooldown);
                weapon.GetValue(GameEntityAPI.FireCommand).AddCondition(cooldown.IsCompleted);
                weapon.GetValue(GameEntityAPI.FireCommand).AddAction(cooldown.ResetTime);
                weapon.WhenFixedTick(cooldown.Tick);
            }
        }
    }
}