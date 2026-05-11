using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class ZombieFireInstaller : IGameEntityInstaller
    {
        [SerializeField] private FireInstaller _fireInstaller;
        [SerializeField] private Cooldown _cooldown;
        
        public void Install(IGameEntity entity)
        {
            _fireInstaller.Install(entity);
            
            entity.GetValue(GameEntityAPI.FireCommand)
                .AddCondition(entity.IsHealthExists)
                .AddCondition(entity.CanFireWithWeapon)
                .AddCondition(entity.IsInAttackDistance)
                .AddCondition(_cooldown.IsCompleted)
                .AddAction(() =>
                {
                    if(_cooldown.IsCompleted())
                        _cooldown.ResetTime();
                });

            entity.WhenFixedTick(_cooldown.Tick);
            
            entity
                .WhenFixedTick(_ => entity.GetValue(GameEntityAPI.FireRequest).Invoke());
        }
    }
}