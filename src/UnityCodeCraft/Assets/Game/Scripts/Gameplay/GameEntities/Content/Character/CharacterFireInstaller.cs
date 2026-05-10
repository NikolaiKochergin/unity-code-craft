using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class CharacterFireInstaller : IGameEntityInstaller
    {
        [SerializeField] private FireInstaller _fireInstaller;
        [SerializeField] private Optional<DownTimer> _fireDelay;
        
        public void Install(IGameEntity entity)
        {
            _fireInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.FireCommand)
                .AddCondition(entity.IsHealthExists)
                .AddCondition(entity.CanFireWithWeapon)
                .AddAction(entity.FireWithWeapon);

            if (_fireDelay)
            {
                ITimer fireDelay = _fireDelay.Value;
                entity.AddValue(GameEntityAPI.FireDelay, fireDelay);
                
                entity
                    .GetValue(GameEntityAPI.FireCommand)
                    .AddCondition(fireDelay.IsCompleted);
                
                entity.WhenFixedTick(deltaTime =>
                {
                    if(entity.IsAiming())
                        fireDelay.Tick(deltaTime);
                    else
                    {
                        fireDelay.Start();
                        fireDelay.ResetTime();
                    }
                });
            }
        }
    }
}