using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;
using Event = Atomic.Elements.Event;

namespace Game.Gameplay
{
    [Serializable]
    public sealed class HealthInstaller : IGameEntityInstaller
    {
        [SerializeField] private Const<int> _maxHealth = 10;
        
        private Subscription<int> _subscription;
        
        public void Install(IGameEntity entity)
        {
            ReactiveVariable<int> currentHealth = new ReactiveVariable<int>(_maxHealth);
            entity.AddValue(GameEntityAPI.CurrentHealth, currentHealth);
            entity.AddValue(GameEntityAPI.MaxHealth, _maxHealth);


            Event deathEvent = new();
            entity.AddValue(GameEntityAPI.DeathEvent, deathEvent);

            _subscription = currentHealth.Subscribe(health =>
            {
                if (health <= 0)
                    deathEvent.Invoke();
            });
        }

        public void Uninstall() => 
            _subscription.Dispose();
    }
}