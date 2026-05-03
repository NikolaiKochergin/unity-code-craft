using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class CharacterInstaller : GameEntityInstaller
    {
        [SerializeField] private TransformInstaller _transformInstaller;
        [SerializeField] private CharacterMoveInstaller _moveInstaller;
        [SerializeField] private AimInstaller _aimInstaller;
        [SerializeField] private RotateInstaller _rotateInstaller;
        [SerializeField] private HealthInstaller _healthInstaller;
        [SerializeField] private TakeDamageInstaller _takeDamageInstaller;
        
        public override void Install(IGameEntity entity)
        {
            _transformInstaller.Install(entity);
            _moveInstaller.Install(entity);
            _rotateInstaller.Install(entity);
            _aimInstaller.Install(entity);
            _healthInstaller.Install(entity);
            _takeDamageInstaller.Install(entity);
            
            entity
                .GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(damage => entity.ReduceHealth(damage));
        }
    }
}