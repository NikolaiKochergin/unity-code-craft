using Atomic.Elements;
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
        [SerializeField] private CharacterFireInstaller _fireInstaller;
        [SerializeField] private GameEntity _weapon;
        [SerializeField] private ReactiveVariable<TeamType> _team;
        
        public override void Install(IGameEntity entity)
        {
            entity.AddTag(GameEntityAPI.CharacterTag);
            entity.AddValue(GameEntityAPI.Transform, transform);
            entity.AddValue(GameEntityAPI.Team, _team);
            entity.AddValue(GameEntityAPI.Weapon, new ReactiveVariable<IGameEntity>(_weapon));
            
            _transformInstaller.Install(entity);
            _moveInstaller.Install(entity);
            _rotateInstaller.Install(entity);
            _aimInstaller.Install(entity);
            _healthInstaller.Install(entity);
            _takeDamageInstaller.Install(entity);
            _fireInstaller.Install(entity);
            
            entity
                .GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(damage => entity.ReduceHealth(damage));
            
            entity
                .GetValue(GameEntityAPI.AimCommand)
                .AddAction(_ => entity.GetValue(GameEntityAPI.FireRequest).Invoke());
        }
    }
}