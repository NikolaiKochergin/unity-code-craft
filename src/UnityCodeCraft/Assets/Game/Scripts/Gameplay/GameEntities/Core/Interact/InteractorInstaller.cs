using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class InteractorInstaller : IGameEntityInstaller
    {
        [SerializeField] private TriggerEvents _triggerEvents;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Trigger, _triggerEvents);
            entity.AddBehaviour<TriggerInteractBehaviour>();
        }
    }
}