using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class AimInstaller : IGameEntityInstaller
    {
        [SerializeField] private Cooldown _aimTime = new (0.04f, 0);
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.AimRequest, new Request<Vector3>());
            entity.AddValue(GameEntityAPI.AimDirection, new Variable<Vector3>());
            
            Command<AimArgs> aimCommand = new();
            aimCommand
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(_ => _aimTime.ResetTime())
                .AddAction(args => entity.RotateStep(args.Direction, args.DeltaTime));
            
            entity.AddValue(GameEntityAPI.AimCommand, aimCommand);
            entity.AddValue(GameEntityAPI.AimTime, _aimTime);

            entity.WhenFixedTick(_aimTime.Tick);

            entity.AddBehaviour<AimBehaviour>();
        }
    }
}