using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class CharacterMoveInstaller : IGameEntityInstaller
    {
        [SerializeField] private MoveInstaller _moveInstaller;

        public void Install(IGameEntity entity)
        {
            _moveInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.MoveCommand)
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(entity.MoveWithAimingOrRotate);
            
            entity.AddValue(GameEntityAPI.MoveSpeedMultiplier, new ReactiveVariable<float>(1f));
        }
    }
}