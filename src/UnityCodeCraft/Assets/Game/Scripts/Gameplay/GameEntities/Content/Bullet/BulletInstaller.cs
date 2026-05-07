using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class BulletInstaller : GameEntityInstaller
    {
        [SerializeField] private TransformInstaller _transformInstaller;
        [SerializeField] private LifetimeInstaller _lifetimeInstaller;

        [SerializeField] private TriggerEvents _triggerEvents;
        [SerializeField] private Const<int> _damage;
        [SerializeField] private Const<int> _moveSpeed = 45;
        [SerializeField] private ReactiveVariable<TeamType> _team;
        
        public override void Install(IGameEntity bullet)
        {
            GameContext gameContext = GameContext.Instance;
            
            _transformInstaller.Install(bullet);
            _lifetimeInstaller.Install(bullet);
            
            bullet.AddValue(GameEntityAPI.Team, _team);
            bullet.AddValue(GameEntityAPI.Trigger, _triggerEvents);
            bullet.AddValue(GameEntityAPI.Damage, _damage);

            bullet.WhenFixedTick(dt => bullet.MoveStepForward(_moveSpeed, dt));
            bullet.AddBehaviour(new BulletCollisionBehaviour(gameContext));
            
            bullet.AddValue(GameEntityAPI.DestroyAction, new InlineAction(bullet.DespawnBullet));
        }
    }
}