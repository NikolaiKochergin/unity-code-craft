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
        [SerializeField] private Const<int> _damage = 1;
        [SerializeField] private Const<int> _moveSpeed = 45;
        [SerializeField] private ReactiveVariable<TeamType> _team;
        
        public override void Install(IGameEntity bullet)
        {
            GameContext gameContext = GameContext.Instance;
            
            _transformInstaller.Install(bullet);
            _lifetimeInstaller.Install(bullet);
            
            bullet.AddValue(GameEntityAPI.Team, _team);

            bullet.WhenFixedTick(dt => bullet.MoveStepForward(_moveSpeed, dt));
            bullet.AddBehaviour(new BulletCollisionBehaviour(gameContext, _damage, _triggerEvents));
            
            bullet.AddValue(GameEntityAPI.DestroyAction, new InlineAction(bullet.DespawnBullet));
        }
    }
}