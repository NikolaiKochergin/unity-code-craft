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
        [SerializeField] private Optional<Cooldown> _fireDelay;
        
        public void Install(IGameEntity entity)
        {
            _fireInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.FireCommand)
                .AddCondition(entity.IsHealthExists)
                .AddCondition(entity.CanFireWithWeapon)
                .AddAction(entity.FireWithWeapon);

            if (_fireDelay) 
                entity.AddValue(GameEntityAPI.FireDelay, _fireDelay.Value);
        }
    }
}