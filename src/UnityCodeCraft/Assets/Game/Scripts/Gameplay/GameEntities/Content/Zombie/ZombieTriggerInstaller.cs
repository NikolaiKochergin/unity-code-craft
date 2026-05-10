using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class ZombieTriggerInstaller : GameEntityInstaller
    {
        [SerializeField] private TriggerEvents _trigger;
        [SerializeField] private GameEntity[] _zombies;
        
        public override void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Trigger, _trigger);
            entity.AddValue(GameEntityAPI.Zombies, _zombies);
            entity.AddBehaviour<DetectCharacterBehaviour>();
        }
    }
}