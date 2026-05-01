using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class RotateInstaller : IGameEntityInstaller
    {
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.RotateRequest, new Request<Vector3>());
            entity.AddValue(GameEntityAPI.RotateCommand, new Command<RotateArgs>());
            entity.AddBehaviour<RotateBehaviour>();
        }
    }
}