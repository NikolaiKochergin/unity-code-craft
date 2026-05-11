using System;
using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    [Serializable]
    public class InteractableInstaller : IGameEntityInstaller
    {
        public void Install(IGameEntity entity)
        {
            entity.AddTag(GameEntityAPI.InteractableTag);
            entity.AddValue(GameEntityAPI.InteractCommand, new Command<IGameEntity>());
        }
    }
}