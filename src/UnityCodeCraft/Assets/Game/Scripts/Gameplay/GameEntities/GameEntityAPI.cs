using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class GameEntityAPI
    {
        public static TagKey<IGameEntity> DamageableTag = new();
        public static TagKey<IGameEntity> MovableTag = new();
        
        public static ValueKey<IGameEntity, IReactiveVariable<int>> CurrentHealth = new(nameof(CurrentHealth));
        public static ValueKey<IGameEntity, IValue<int>> MaxHealth = new(nameof(MaxHealth));
        public static ValueKey<IGameEntity, IEvent> DeathEvent = new(nameof(DeathEvent));
        public static ValueKey<IGameEntity, Animator> Animator = new(nameof(Animator));
        public static ValueKey<IGameEntity, ICommand<int>> TakeDamageCommand = new(nameof(TakeDamageCommand));
        public static ValueKey<IGameEntity, ICommand<MoveArgs>> MoveCommand = new(nameof(MoveCommand));
        public static ValueKey<IGameEntity, IRequest<Vector3>> MoveRequest = new(nameof(MoveRequest));
        public static ValueKey<IGameEntity, ICommand<RotateArgs>> RotateCommand = new(nameof(RotateCommand));
        public static ValueKey<IGameEntity, IRequest<Vector3>> RotateRequest = new(nameof(RotateRequest));
        public static ValueKey<IGameEntity, IVariable<Quaternion>> Rotation = new(nameof(Rotation));
        public static ValueKey<IGameEntity, IValue<float>> RotateSpeed = new(nameof(RotateSpeed));
        public static ValueKey<IGameEntity, IValue<Vector3>> Position = new(nameof(Position));
        public static ValueKey<IGameEntity, IRequest<Vector3>> AimRequest = new(nameof(AimRequest));
        public static ValueKey<IGameEntity, ICommand<AimArgs>> AimCommand = new(nameof(AimCommand));
        public static ValueKey<IGameEntity, IVariable<Vector3>> AimDirection = new(nameof(AimDirection));
        public static ValueKey<IGameEntity, ICooldown> AimTime = new(nameof(AimTime));
        
        public static ValueKey<ICooldown> MoveTime = new(nameof(MoveTime));
        public static ValueKey<IGameEntity, IReactiveVariable<float>> MoveSpeedMultiplier = new(nameof(MoveSpeedMultiplier));
    }
}