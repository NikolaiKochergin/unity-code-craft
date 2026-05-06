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

                .AddAction(entity.FireWithWeapon);

            if (_fireDelay)
            {
                ITimer fireDelay = _fireDelay.Value;
                entity.AddValue(GameEntityAPI.FireDelay, fireDelay);
                // TODO: тут дописать механику задержки перед стрельбой
            }
        }
    }
}