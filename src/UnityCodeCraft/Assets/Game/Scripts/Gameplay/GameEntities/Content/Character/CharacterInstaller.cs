using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class CharacterInstaller : GameEntityInstaller
    {
        [SerializeField] private TransformInstaller _transformInstaller;
        [SerializeField] private CharacterMoveInstaller _moveInstaller;
        [SerializeField] private RotateInstaller _rotateInstaller;
        [SerializeField] private HealthInstaller _healthInstaller;
        [SerializeField] private TakeDamageInstaller _takeDamageInstaller;

        [SerializeField] private Const<float> _rotateSpeed = 720;
        
        public override void Install(IGameEntity entity)
        {
            _transformInstaller.Install(entity);
            _moveInstaller.Install(entity);
            _healthInstaller.Install(entity);
            _takeDamageInstaller.Install(entity);
            
            entity
                .GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(damage => entity.ReduceHealth(damage));
            
            InstallRotation(entity);
        }

        private void InstallRotation(IGameEntity entity)
        {
            _rotateInstaller.Install(entity);
            entity.GetValue(GameEntityAPI.RotateCommand)
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(entity.RotateStep);
            
            entity.AddValue(GameEntityAPI.RotateSpeed, _rotateSpeed);
        }
    }
}