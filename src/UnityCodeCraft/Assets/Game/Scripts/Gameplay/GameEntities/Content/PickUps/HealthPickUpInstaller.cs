using Atomic.Entities;
using UnityEngine;
using Event = Atomic.Elements.Event;

namespace Game.Gameplay
{
    public class HealthPickUpInstaller : GameEntityInstaller
    {
        [SerializeField] private int _health = 3;
        [SerializeField] private Collider _collider;
        [SerializeField] private InteractableInstaller _interactableInstaller;
        
        public override void Install(IGameEntity entity)
        {
            _interactableInstaller.Install(entity);

            Event collectedEvent = new();
            entity.AddValue(GameEntityAPI.CollectedEvent, collectedEvent);

            entity.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(target => target.HasTag(GameEntityAPI.CharacterTag))
                .AddAction(target =>
                {
                    if (target.CollectHealth(_health))
                    {
                        _collider.enabled = false;
                        collectedEvent.Invoke();
                    }
                });
            
        }
    }
}