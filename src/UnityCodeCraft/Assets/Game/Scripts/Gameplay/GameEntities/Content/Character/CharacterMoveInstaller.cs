using System;
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
                .AddAction(args =>
                {
                    if (entity.IsAiming())
                        entity.GetValue(GameEntityAPI.AimDirection).Value =
                            Quaternion.Inverse(entity.GetValue(GameEntityAPI.Rotation).Value) *
                            args.Direction.normalized;
                    else
                        entity.RotateStep(args.Direction, args.DeltaTime);
                });
        }
    }
}