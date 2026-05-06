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
        public static ValueKey<IGameEntity, ICooldown> MoveTime = new(nameof(MoveTime));
        public static ValueKey<IGameEntity, ICommand<RotateArgs>> RotateCommand = new(nameof(RotateCommand));
        public static ValueKey<IGameEntity, IRequest<Vector3>> RotateRequest = new(nameof(RotateRequest));
        public static ValueKey<IGameEntity, IVariable<Quaternion>> Rotation = new(nameof(Rotation));
        public static ValueKey<IGameEntity, IValue<float>> RotateSpeed = new(nameof(RotateSpeed));
        public static ValueKey<IGameEntity, IVariable<Vector3>> Position = new(nameof(Position));
        public static ValueKey<IGameEntity, IRequest<Vector3>> AimRequest = new(nameof(AimRequest));
        public static ValueKey<IGameEntity, ICommand<AimArgs>> AimCommand = new(nameof(AimCommand));
        public static ValueKey<IGameEntity, IVariable<Vector3>> AimDirection = new(nameof(AimDirection));
        public static ValueKey<IGameEntity, ICooldown> AimTime = new(nameof(AimTime));
        public static ValueKey<IGameEntity, ICommand> FireCommand = new(nameof(FireCommand));
        public static ValueKey<IGameEntity, IRequest> FireRequest = new(nameof(FireRequest));
        public static ValueKey<IGameEntity, IReactiveVariable<IGameEntity>> Weapon = new(nameof(Weapon));
        
        public static ValueKey<IGameEntity, IReactiveVariable<float>> MoveSpeedMultiplier = new(nameof(MoveSpeedMultiplier));
        public static ValueKey<IGameEntity, ITimer> FireDelay = new(nameof(FireDelay));
        public static ValueKey<IGameEntity, IReactiveVariable<int>> Ammo = new(nameof(Ammo));
        public static ValueKey<IGameEntity, ICooldown> FireCooldown = new(nameof(FireCooldown));
        public static ValueKey<IGameEntity, GameEntity> BulletPrefab = new(nameof(BulletPrefab));
        public static ValueKey<IGameEntity, ICooldown> Lifetime = new(nameof(Lifetime));
        public static ValueKey<IGameEntity, IAction> DestroyAction = new(nameof(DestroyAction));
        public static ValueKey<IGameEntity, IValue<int>> Damage = new(nameof(Damage));
    }
}