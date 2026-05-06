using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class BulletInstaller : GameEntityInstaller
    {
        [SerializeField] private TransformInstaller _transformInstaller;
        [SerializeField] private LifetimeInstaller _lifetimeInstaller;

        [SerializeField] private Const<int> _damage;
        [SerializeField] private Const<int> _moveSpeed = 45;
        
        public override void Install(IGameEntity bullet)
        {
            _transformInstaller.Install(bullet);
            _lifetimeInstaller.Install(bullet);
            
            bullet.AddValue(GameEntityAPI.Damage, _damage);

            bullet.WhenFixedTick(dt => bullet.MoveStepForward(_moveSpeed, dt));
            
            bullet.AddValue(GameEntityAPI.DestroyAction, new InlineAction(bullet.DespawnBullet));
        }
    }
}