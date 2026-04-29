using System;
using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    [Serializable]
    public sealed class TakeDamageInstaller : IGameEntityInstaller
    {
        public void Install(IGameEntity entity)
        {
            entity.AddTag(GameEntityAPI.DamageableTag);
            entity.AddValue(GameEntityAPI.TakeDamageCommand, new Command<int>());
        }
    }
}