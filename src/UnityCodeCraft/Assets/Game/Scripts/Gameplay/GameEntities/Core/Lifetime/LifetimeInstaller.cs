using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public sealed class LifetimeInstaller : IGameEntityInstaller
    {
        [SerializeField] private Cooldown _cooldown = 3;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Lifetime, _cooldown);
            entity.AddBehaviour<LifetimeBehaviour>();
        }
    }
}