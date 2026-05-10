using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class ZombieInstaller : GameEntityInstaller
    {
        [SerializeField] private GameEntity _initialTarget;
        [SerializeField] private Collider _collider;
        [SerializeField] private TransformInstaller _transformInstaller;
        [SerializeField] private HealthInstaller _healthInstaller;
        [SerializeField] private TakeDamageInstaller _takeDamageInstaller;
        [SerializeField] private MoveInstaller _moveInstaller;
        [SerializeField] private RotateInstaller _rotateInstaller;
        [SerializeField] private Const<float> _attackDistance;
        [SerializeField] private FireInstaller _fireInstaller;
        [SerializeField] private ReactiveVariable<TeamType> _team = TeamType.ENEMY;
        
        private readonly DisposableComposite _disposables = new();
        
        public override void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Team, _team);
            entity.AddValue(GameEntityAPI.AttackDistance, _attackDistance);
            _transformInstaller.Install(entity);
            _healthInstaller.Install(entity);
            _rotateInstaller.Install(entity);
            _fireInstaller.Install(entity);
            
            InstallTakeDamage(entity);
            InstallMove(entity);
            
            entity.AddValue(GameEntityAPI.Target, new Variable<IGameEntity>(_initialTarget));
        }

        private void InstallMove(IGameEntity entity)
        {
            _moveInstaller.Install(entity);
            entity
                .GetValue(GameEntityAPI.MoveCommand)
                .AddAction(args => entity.RotateStep(args.Direction, args.DeltaTime));
            
            entity.AddBehaviour<MoveToTargetBehaviour>();
        }

        private void InstallTakeDamage(IGameEntity entity)
        {
            _takeDamageInstaller.Install(entity);
            entity.GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(damage => entity.ReduceHealth(damage));
            
            entity
                .GetValue(GameEntityAPI.CurrentHealth)
                .Subscribe(health => _collider.enabled = health > 0)
                .AddTo(_disposables);
        }

        public override void Uninstall(IGameEntity entity)
        {
            _disposables.Dispose();
        }
    }
}