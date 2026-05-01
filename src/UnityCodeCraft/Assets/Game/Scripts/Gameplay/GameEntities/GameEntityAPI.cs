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
        public static ValueKey<IGameEntity, ICommand<MoveArgs>> MoveCommand = new(nameof(MoveCommand));
        public static ValueKey<IGameEntity, IRequest<Vector3>> MoveRequest = new(nameof(MoveRequest));
        public static ValueKey<IGameEntity, Command<RotateArgs>> RotateCommand = new(nameof(RotateCommand));
        public static ValueKey<IGameEntity, IRequest<Vector3>> RotateRequest = new(nameof(RotateRequest));
        public static ValueKey<IGameEntity, IVariable<Quaternion>> Rotation = new(nameof(Rotation));
        public static ValueKey<IGameEntity, IValue<float>> RotateSpeed = new(nameof(RotateSpeed));
        public static ValueKey<IGameEntity, IValue<Vector3>> Position = new(nameof(Position));
        
        public static ValueKey<ICooldown> MoveTime = new(nameof(MoveTime));
    }
}