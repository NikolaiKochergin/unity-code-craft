using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;
using Event = Atomic.Elements.Event;

namespace Game.Gameplay
{
    public static class GameEntityAPI
    {
        public static TagKey<IGameEntity> DamageableTag = new();
        public static TagKey<IGameEntity> MovableTag = new();
        
        public static ValueKey<IGameEntity, IReactiveVariable<int>> CurrentHealth = new(nameof(CurrentHealth));
        public static ValueKey<IGameEntity, Const<int>> MaxHealth = new(nameof(MaxHealth));
        public static ValueKey<IGameEntity, Event> DeathEvent = new(nameof(DeathEvent));
        public static ValueKey<IGameEntity, Animator> Animator = new(nameof(Animator));
        public static ValueKey<IGameEntity, Command<int>> TakeDamageCommand = new(nameof(TakeDamageCommand));
        public static ValueKey<IGameEntity, IVariable<Vector3>> Position = new(nameof(Position));
    }
}