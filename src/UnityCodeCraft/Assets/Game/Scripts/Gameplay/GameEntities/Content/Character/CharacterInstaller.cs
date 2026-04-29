using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class CharacterInstaller : GameEntityInstaller
    {
        [SerializeField] private CharacterMoveInstaller _moveInstaller;
        [SerializeField] private HealthInstaller _healthInstaller;
        [SerializeField] private TakeDamageInstaller _takeDamageInstaller;
        
        public override void Install(IGameEntity entity)
        {
            _moveInstaller.Install(entity);
            _healthInstaller.Install(entity);
            _takeDamageInstaller.Install(entity);
            
            entity
                .GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsHealthExists())
                .AddAction(damage => entity.ReduceHealth(damage));
        }
    }
}