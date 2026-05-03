using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class AimInstaller : IGameEntityInstaller
    {
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.AimRequest, new Request<Vector3>());
            entity.AddValue(GameEntityAPI.AimDirection, new Variable<Vector3>());
            
            Command<AimArgs> aimCommand = new();
            aimCommand
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(args =>
                {
                    entity.RotateStep(args.Direction, args.DeltaTime);
                });
            
            // TODO: РАЗОБРАТЬСЯ КАК ПРАВИЛЬНО ЗАПИСАТЬ ЭТО НАПРАВЛЕНИЕ
                // .AddAction(args => entity.GetValue(GameEntityAPI.AimDirection).Value = 
                //     Vector3.Cross(args.Direction, entity.GetValue(GameEntityAPI.ro)));
            
            entity.AddValue(GameEntityAPI.AimCommand, aimCommand);

            entity.AddBehaviour<AimBehaviour>();
        }
    }
}