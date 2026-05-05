using System;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class CharacterFireInstaller : IGameEntityInstaller
    {
        [SerializeField] private FireInstaller _fireInstaller;
        
        public void Install(IGameEntity entity)
        {
            _fireInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.FireCommand)
                .AddCondition(entity.IsHealthExists)

                .AddAction(entity.FireWithWeapon);
        }
    }
}