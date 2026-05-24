using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class WeaponInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField] private GameEntity _owner;
        [SerializeField] private Optional<ReactiveVariable<int>> _ammo;
        [SerializeField] private Optional<Const<int>> _maxAmmo;
        [SerializeField] private Optional<Cooldown> _cooldown;
        
        public override void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Owner, new Variable<IGameEntity>(_owner));
            entity.AddValue(GameEntityAPI.FireCommand, new Command());
            
            if (_ammo)
            {
                IReactiveVariable<int> ammo = _ammo.Value;
                entity.AddValue(GameEntityAPI.Ammo, ammo);
                entity.GetValue(GameEntityAPI.FireCommand).AddCondition(() => ammo.Value > 0);
                entity.GetValue(GameEntityAPI.FireCommand).AddAction(() => ammo.Value--);
            }
            
            if (_cooldown)
            {
                ICooldown cooldown = _cooldown.Value;
                entity.AddValue(GameEntityAPI.FireCooldown, cooldown);
                entity.GetValue(GameEntityAPI.FireCommand).AddCondition(cooldown.IsCompleted);
                entity.GetValue(GameEntityAPI.FireCommand).AddAction(cooldown.ResetTime);
                entity.WhenFixedTick(cooldown.Tick);
            }
            
            if(_maxAmmo)
                entity.AddValue(GameEntityAPI.MaxAmmo, _maxAmmo.Value);
        }
    }
}