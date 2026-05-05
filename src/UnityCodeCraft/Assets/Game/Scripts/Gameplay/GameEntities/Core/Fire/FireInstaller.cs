using System;
using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    [Serializable]
    public sealed class FireInstaller : IGameEntityInstaller
    {
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.FireRequest, new Request());
            entity.AddValue(GameEntityAPI.FireCommand, new Command());
            entity.AddBehaviour<FireBehaviour>();
        }
    }
}