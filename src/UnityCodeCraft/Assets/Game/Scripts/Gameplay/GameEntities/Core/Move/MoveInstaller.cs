using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class MoveInstaller : IGameEntityInstaller
    {
        [SerializeField] private Cooldown _moveTime = new (0.04f, 0);
        
        public void Install(IGameEntity entity)
        {
            entity.AddTag(GameEntityAPI.MovableTag);
            entity.AddValue(GameEntityAPI.MoveRequest, new Request<Vector3>());

            Command<MoveArgs> moveCommand = new();
            moveCommand.AddAction(_ => _moveTime.ResetTime());
            entity.AddValue(GameEntityAPI.MoveCommand, moveCommand);
            
            entity.AddValue(GameEntityAPI.MoveTime, _moveTime);
            entity.WhenFixedTick(_moveTime.Tick);
            
            entity.AddBehaviour<MoveBehaviour>();
        }
    }
}