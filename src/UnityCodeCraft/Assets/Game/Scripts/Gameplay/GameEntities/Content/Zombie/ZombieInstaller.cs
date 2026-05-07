using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class ZombieInstaller : GameEntityInstaller
    {
        [SerializeField] private TransformInstaller _transformInstaller;
        [SerializeField] private HealthInstaller _healthInstaller;
        [SerializeField] private TakeDamageInstaller _takeDamageInstaller;
        [SerializeField] private ReactiveVariable<TeamType> _team = TeamType.ENEMY;
        
        public override void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Team, _team);
            _transformInstaller.Install(entity);
            _healthInstaller.Install(entity);
            
            InstallTakeDamage(entity);
        }

        private void InstallTakeDamage(IGameEntity entity)
        {
            _takeDamageInstaller.Install(entity);
            entity.GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(damage =>
                {
                    entity.ReduceHealth(damage);
                    if(entity.IsDead())
                        gameObject.SetActive(false);
                });
        }
    }
}